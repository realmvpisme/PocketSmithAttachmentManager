using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;
using Type = System.Type;

namespace PocketSmith.DataExportServices
{
    /// <summary>
    /// Cleans up database entries that do not have a matching entry on the API.
    /// Methods in this class should only be run after all entity updates are complete in order to prevent cascading SQL failures.
    /// </summary>
    public class DatabaseCleanupService
    {
        private readonly ContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly string _databaseFilePath;
        private readonly IMemoryCache _memoryCache;
        private readonly List<string> _cacheKeys;


        public DatabaseCleanupService(string databaseFilePath)
        {
            _contextFactory = new ContextFactory();

            if (string.IsNullOrEmpty(databaseFilePath))
            {
                throw new ArgumentNullException("databaseFilePath cannot be null.");
            }
            _databaseFilePath = databaseFilePath;
            _mapper = new Mapper(MapperConfigurationGenerator.Invoke());

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cacheKeys = new List<string>();
        }

        public void QueueCleanupEntity<TDisplayModel, TDatabaseModel>(TDisplayModel item)
        {
            var dbEntity = _mapper.Map<TDatabaseModel>(item);
            var idValue = typeof(TDatabaseModel).GetProperty("Id")?.GetValue(dbEntity);
            var key = typeof(TDatabaseModel).FullName + "_" + idValue;
            if (_memoryCache.TryGetValue(key, out object existingCachedRecord))
            {
                return;
            }
            _memoryCache.Set(
                key, 
                dbEntity,
                new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _cacheKeys.Add(key);
        }

        public async Task CleanUpDatabase()
        {
            Console.WriteLine("Cleaning up database...");
            try
            {
                foreach (var key in _cacheKeys)
                {
                    var cachedValueExists = _memoryCache.TryGetValue(key, out var cachedValue);
                    if (cachedValueExists)
                    {
                        var typeName = Regex.Match(key, "[a-z].+(?=_)", RegexOptions.IgnoreCase).Value;
                        var assembly = Assembly.GetAssembly(typeof(DB_Transaction));
                        var entityType = assembly.GetType(typeName);
                        var entityIdType = assembly.GetType(typeName).GetProperty("Id").GetValue(cachedValue).GetType();
                        var entityId = cachedValue.GetType().GetProperty("Id").GetValue(cachedValue);
                        var deleteMethod = GetType().GetMethod("DeleteEntity").MakeGenericMethod(new Type[]{entityType, entityIdType});

                        if (deleteMethod != null)
                        {
                            deleteMethod.Invoke(this, new object[] { entityId });
                            _memoryCache.Remove(key);
                        }
                    }
                }
            }
            catch (Exception e)
            {
               Console.WriteLine("Cleanup failed. An error occurred.");
            }
           

            Console.WriteLine("Cleanup finished!");
        }

        public async Task DeleteEntity<TDatabaseModel, TEntityId>(TEntityId entityId)
        where TDatabaseModel : ModelBase<TEntityId>
        {
            await using var context = _contextFactory.Create(_databaseFilePath);

            var selectedEntity = await context.Set<TDatabaseModel>().FindAsync(entityId);
            if (selectedEntity != null)
            {
                if (selectedEntity is ISoftDeletable)
                {
                    ((ISoftDeletable)selectedEntity).Deleted = true;
                    selectedEntity.LastUpdated = DateTime.UtcNow;
                    context.Update(selectedEntity);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.Remove(selectedEntity);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
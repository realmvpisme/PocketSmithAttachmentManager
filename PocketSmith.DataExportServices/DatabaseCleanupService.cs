using System;
using System.Collections.Generic;
using System.Linq;
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

        public void QueueCleanupEntity<TDeletableItem>(TDeletableItem item)
        {
            var idValue = typeof(TDeletableItem).GetProperty("Id")?.GetValue(item);
            var key = typeof(TDeletableItem).FullName + "_" + idValue;
            _memoryCache.Set(
                key, 
                item,
                new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _cacheKeys.Add(key);
        }

        public async Task CleanUpDatabase()
        {
            Console.WriteLine("Cleaning up database.,.");
            foreach (var key in _cacheKeys)
            {
                var cachedValueExists = _memoryCache.TryGetValue(key, out var cacheValue);
                if (cachedValueExists)
                {
                    var typeName = Regex.Match(key, "[a-z].+(?=_)", RegexOptions.IgnoreCase).Value;
                    try
                    {
                        var typedCacheValue = Convert.ChangeType(cacheValue, Type.GetType(typeName));
                        var idValue = typedCacheValue.GetType().GetProperty("Id").GetValue(typedCacheValue);
                        await deleteEntity(typedCacheValue, idValue);
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }

            Console.WriteLine("Cleanup finished!");
        }

        private async Task deleteEntity<TDatabaseModel, TEntityId>(TDatabaseModel entity, TEntityId id)
        where TDatabaseModel : ModelBase<TEntityId>
        {
            await using var context = _contextFactory.Create(_databaseFilePath);

            var selectedEntity = await context.Set<TDatabaseModel>().FindAsync((object)id);
            if (selectedEntity != null)
            {
                context.Remove(selectedEntity);
                await context.SaveChangesAsync();
            }
        }
    }
}
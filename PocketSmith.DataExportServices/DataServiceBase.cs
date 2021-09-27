using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketSmith.DataExportServices
{
    public abstract class DataServiceBase<TJsonModel, TDatabaseModel, TEntityId>
    where TDatabaseModel : ModelBase<TEntityId>
    {
        protected readonly Mapper Mapper;
        protected readonly ContextFactory ContextFactory;
        protected readonly string DatabaseFilePath;

        protected DataServiceBase(string databaseFilePath)
        {
            Mapper = new Mapper(MapperConfigurationGenerator.Invoke());
            ContextFactory = new ContextFactory();

            if (string.IsNullOrEmpty(databaseFilePath))
            {
                throw new ArgumentNullException("databaseFilePath cannot be null.");
            }

            DatabaseFilePath = databaseFilePath;
        }
        public virtual async Task<TJsonModel> Create(TJsonModel createItem)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);
            var dbEntity = Mapper.Map<TDatabaseModel>(createItem);
            dbEntity.CreatedTime = DateTime.UtcNow;
            dbEntity.LastUpdated = DateTime.UtcNow;


            try
            {
                var createResult = await context.AddAsync(dbEntity);
                await context.SaveChangesAsync();
                return await GetById(createResult.Entity.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        public virtual async Task Update(TJsonModel updateItem, TEntityId id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FindAsync((object)id);

            if (dbEntity == null)
            {
                return;
            }

            var updatedEntity = Mapper.Map(updateItem, dbEntity);
            updatedEntity.LastUpdated = DateTime.UtcNow;
            try
            {
                
                context.Entry(updatedEntity).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public virtual async Task Delete(TEntityId id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FindAsync((object)id);

            if (dbEntity.GetType() == typeof(ISoftDeletable))
            {
                dbEntity.LastUpdated = DateTime.UtcNow;
                ((ISoftDeletable)dbEntity).Deleted = true;
                context.Update(dbEntity);
                await context.SaveChangesAsync();
                return;
            }

            context.Remove(dbEntity);
            await context.SaveChangesAsync();
        }

        public virtual async Task<List<TJsonModel>> GetAll()
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntities = await context.Set<TDatabaseModel>().ToListAsync();
            var mappedEntities = Mapper.Map<List<TJsonModel>>(dbEntities);

            return mappedEntities;
        }

        public virtual async Task<TJsonModel> GetById(TEntityId id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var mappedEntity = Mapper.Map<TJsonModel>(dbEntity);
            return mappedEntity;
        }

        public virtual async Task<bool> Exists(TEntityId id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            return await context.Set<TDatabaseModel>().AnyAsync(x => x.Id.Equals(id));
        }
    }
}
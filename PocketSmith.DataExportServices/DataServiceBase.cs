using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketSmith.DataExportServices
{
    public abstract class DataServiceBase<TJsonModel, TDatabaseModel>
    where TDatabaseModel : ModelBase
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

            var createResult = await context.AddAsync(dbEntity);
            await context.SaveChangesAsync();
            return await GetById(createResult.Entity.Id);
        }

        public virtual async Task Update(TJsonModel updateItem, long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FindAsync((object)id);

            if (dbEntity == null)
            {
                return;
            }

            var updatedEntity = Mapper.Map(updateItem, dbEntity);
            context.Entry(updatedEntity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public virtual async Task Delete(long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id == id);
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

        public virtual async Task<TJsonModel> GetById(long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id == id);
            var mappedEntity = Mapper.Map<TJsonModel>(dbEntity);
            return mappedEntity;
        }

        public virtual async Task<bool> Exists(long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            return await context.Set<TDatabaseModel>().AnyAsync(x => x.Id == id);
        }
    }
}
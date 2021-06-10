using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Models;

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
            return await GetById(createResult.Entity.Id);
        }

        public virtual async Task Update(TJsonModel updateItem, long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id == id);
            dbEntity = Mapper.Map<TDatabaseModel>(updateItem);
            context.Update(dbEntity);
        }

        public virtual async Task Delete(long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = await context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id == id);
            context.Remove(dbEntity);
        }

        public virtual async Task<TJsonModel> GetById(long id)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);

            var dbEntity = context.Set<TDatabaseModel>().FirstOrDefaultAsync(x => x.Id == id);
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
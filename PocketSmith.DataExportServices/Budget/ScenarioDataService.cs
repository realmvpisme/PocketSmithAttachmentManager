using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Budget
{
    public class ScenarioDataService : DataServiceBase<ScenarioModel, DB_Scenario, long>
    {
        public ScenarioDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }

        public async Task<IEnumerable<ScenarioModel>> GetAllByAccountIds(IEnumerable<long> idList)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);
            var mappedEntity = await context.Scenarios.Where(x => idList.Contains(x.Id))
                .ProjectTo<ScenarioModel>(Mapper.ConfigurationProvider).ToListAsync();
            return mappedEntity;
        }
    }
}
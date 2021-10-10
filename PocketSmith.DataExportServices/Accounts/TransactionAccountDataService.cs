using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts
{
    public class TransactionAccountDataService : DataServiceBase<TransactionAccountModel, DB_TransactionAccount, long>
    {
        public TransactionAccountDataService(string databaseFilePath) : base(databaseFilePath)
        {

        }

        public async Task<IEnumerable<TransactionAccountModel>> GetAllByAccountIds(IEnumerable<long> idList)
        {
            await using var context = ContextFactory.Create(DatabaseFilePath);
            var mappedEntities = await context.TransactionAccounts.Where(x => idList.Contains(x.Id))
                .ProjectTo<TransactionAccountModel>(Mapper.ConfigurationProvider).ToListAsync();
            return mappedEntities;
        }

    }
}
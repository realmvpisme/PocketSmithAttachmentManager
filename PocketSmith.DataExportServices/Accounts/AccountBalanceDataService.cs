using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PocketSmith.DataExport;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts
{
    public class AccountBalanceDataService
    {
        private readonly Mapper _mapper;
        private readonly ContextFactory _contextFactory;
        private readonly string _databaseFilePath;

        public AccountBalanceDataService(string databaseFilePath)
        {
            _databaseFilePath = databaseFilePath;
        }

        public async Task<AccountBalanceModel> Create(AccountBalanceModel createItem)
        {
            await using var context = _contextFactory.Create(_databaseFilePath);

            var existingBalance = await context.AccountBalances.FirstOrDefaultAsync(x =>
                x.AccountId == createItem.AccountId && x.Balance == createItem.Balance && x.Date == createItem.Date);

            if (existingBalance != null)
            {
                return _mapper.Map<AccountBalanceModel>(existingBalance);
            }

            var dbEntity = _mapper.Map<DB_AccountBalance>(createItem);

            var createResult = await context.AddAsync(dbEntity);
            await context.SaveChangesAsync();
            return await GetById(createResult.Entity.Id);
        }

        public async Task<AccountBalanceModel> GetById(long id)
        {
            await using var context = _contextFactory.Create(_databaseFilePath);

            var dbEntity = await context.Set<DB_AccountBalance>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var mappedEntity = _mapper.Map<AccountBalanceModel>(dbEntity);
            return mappedEntity;

        }
    }
}
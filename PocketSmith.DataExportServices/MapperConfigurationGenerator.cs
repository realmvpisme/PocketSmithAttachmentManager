using AutoMapper;
using PocketSmith.DataExportServices.Accounts.Mappers;
using PocketSmith.DataExportServices.Budget.Mappers;
using PocketSmith.DataExportServices.Categories.Mappers;
using PocketSmith.DataExportServices.Institutions.Mappers;
using PocketSmith.DataExportServices.Transactions.Mappers;

namespace PocketSmith.DataExportServices
{
    public static class MapperConfigurationGenerator
    {
        public static MapperConfiguration Invoke()
        {
            return new MapperConfiguration(cfg =>
            {
               cfg.AddProfile(new TransactionMapperProfile());
               cfg.AddProfile(new AccountMapperProfile());
               cfg.AddProfile(new CategoryMapperProfile());
               cfg.AddProfile(new InstitutionMapperProfile());
               cfg.AddProfile(new ScenarioMapperProfile());
               cfg.AddProfile(new BudgetEventMapperProfile());
            });
        }
    }
}
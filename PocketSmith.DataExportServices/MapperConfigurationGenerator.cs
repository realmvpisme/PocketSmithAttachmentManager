using AutoMapper;
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
            });
        }
    }
}
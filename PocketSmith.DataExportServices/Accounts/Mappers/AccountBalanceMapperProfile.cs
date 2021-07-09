using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts.Mappers
{
    public class AccountBalanceMapperProfile : Profile
    {
        public AccountBalanceMapperProfile()
        {
            CreateMap<AccountBalanceModel, DB_AccountBalance>()
                .ReverseMap();
        }
    }
}
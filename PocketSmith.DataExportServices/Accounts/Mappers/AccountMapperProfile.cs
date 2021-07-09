using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts.Mappers
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile()
        {
            CreateMap<AccountModel, DB_Account>()
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                })
                .ForMember(dest => dest.PrimaryScenarioId, map =>
                {
                    map.MapFrom(src => src.PrimaryScenario.Id);
                })
                .ForMember(dest => dest.PrimaryTransactionAccountId, map =>
                {
                    map.MapFrom(src => src.PrimaryTransactionAccount.Id);
                })
                .ForMember(dest => dest.PrimaryScenario, map =>
                {
                    map.Ignore();
                })
                .ForMember(dest => dest.PrimaryTransactionAccount, map =>
                {
                    map.Ignore();
                }).ReverseMap();
        }
    }
}
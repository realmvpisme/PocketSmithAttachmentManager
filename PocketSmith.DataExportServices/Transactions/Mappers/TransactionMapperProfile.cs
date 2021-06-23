using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Transactions.Mappers
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<TransactionModel, DB_Transaction>()
                .ForMember(dest => dest.AccountId, x =>
            {
                x.MapFrom(src => src.TransactionAccount.Id);
            })
                .ForMember(dest => dest.CategoryId, x =>
                {
                    x.MapFrom(
                        src => src.Category.Id);
                })
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                })
                .ForMember(dest => dest.Category, map => map.Ignore())
                .ForMember(dest => dest.Account, map => map.Ignore()).ReverseMap();
        }
    }
}
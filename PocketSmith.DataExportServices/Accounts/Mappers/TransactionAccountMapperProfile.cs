using System.Xml;
using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Accounts.Mappers
{
    public class TransactionAccountMapperProfile : Profile
    {
        public TransactionAccountMapperProfile()
        {
            CreateMap<TransactionAccountModel, DB_TransactionAccount>()
                .ForMember(dest => dest.InstitutionId, map =>
                {
                    map.MapFrom(src => src.Institution.Id);
                })
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                })
                .ForMember(dest => dest.Institution, map => map.Ignore())
                .ReverseMap();

            
        }
    }
}
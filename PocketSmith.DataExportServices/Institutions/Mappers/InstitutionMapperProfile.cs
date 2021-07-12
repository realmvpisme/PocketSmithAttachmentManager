using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Institutions.Mappers
{
    public class InstitutionMapperProfile : Profile
    {
        public InstitutionMapperProfile()
        {
            CreateMap<InstitutionModel, DB_Institution>()
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                }).ReverseMap();
        }
    }
}
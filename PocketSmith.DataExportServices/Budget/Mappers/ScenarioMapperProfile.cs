using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Budget.Mappers
{
    public class ScenarioMapperProfile : Profile
    {
        public ScenarioMapperProfile()
        {
            CreateMap<ScenarioModel, DB_Scenario>()
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                }).ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                }).ReverseMap();
        }
    }
}
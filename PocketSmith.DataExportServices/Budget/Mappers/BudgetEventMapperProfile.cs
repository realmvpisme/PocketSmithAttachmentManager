using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Budget.Mappers
{
    public class BudgetEventMapperProfile : Profile
    {
        public BudgetEventMapperProfile()
        {
            CreateMap<BudgetEventModel, DB_BudgetEvent>()
                .ForMember(dest => dest.CategoryId, map =>
                {
                    map.MapFrom(src => src.Category.Id);
                })
                .ForMember(dest => dest.ScenarioId, map =>
                {
                    map.MapFrom(src => src.Scenario.Id);
                })
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.Scenario.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.Scenario.UpdatedAt);
                })
                .ForMember(dest => dest.Category, map =>
                {
                    map.Ignore();
                })
                .ForMember(dest => dest.Scenario, map =>
                {
                    map.Ignore();
                }).ReverseMap();
        }
    }
}
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
                }).ReverseMap();
        }
    }
}
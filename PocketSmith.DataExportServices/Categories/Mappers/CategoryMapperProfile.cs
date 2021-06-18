using AutoMapper;
using PocketSmith.DataExport.Models;
using PocketSmith.DataExportServices.JsonModels;

namespace PocketSmith.DataExportServices.Categories.Mappers
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<CategoryModel, DB_Category>()
                .ForMember(dest => dest.CreatedTime, map =>
                {
                    map.MapFrom(src => src.CreatedAt);
                })
                .ForMember(dest => dest.LastUpdated, map =>
                {
                    map.MapFrom(src => src.UpdatedAt);
                })
                .ForMember(dest => dest.Color, map =>
                {
                    map.MapFrom(src => src.Colour);
                })
                .ForMember(dest => dest.ParentId, map =>
                {
                    map.AllowNull();
                    map.MapFrom(src => src.ParentId);
                })
                .ReverseMap();
        }
    }
}
using GeoJSON.Net.Contrib.MsSqlSpatial;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using GeoJSON.Net.Feature;
using GeoAPI.Geometries;
using AutoMapper;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<IGeometry, Feature>().ConvertUsing(x => new Feature(WktConvert.GeoJSONGeometry(x.ToString(), 4326), null, null));
                       
            CreateMap<Category, CategoryDto>();

            CreateMap<Area, AreaDto>()
              .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));

            CreateMap<PointOfInterest, PointOfInterestDto>()
              .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location))
              .ForMember(dest => dest.Area, opt => opt.Ignore());            

            CreateMap<Route, RouteDto>()
              .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));

            CreateMap<Segment, SegmentDto>()
              .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));
        }
    }
}
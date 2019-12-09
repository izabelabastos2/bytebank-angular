using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using GeoJSON.Net.Feature;
using AutoMapper;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {
            CreateMap<GeoJSON.Net.Geometry.MultiPolygon, Geometry>();
            CreateMap<GeoJSON.Net.Geometry.LineString, Geometry>();
            CreateMap<GeoJSON.Net.Geometry.Point, Geometry>();

            CreateMap<Feature, Area>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Properties["Name"]))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Properties["description"]))
           .ForMember(x => x.Location, opt => opt.Ignore())
           .ForMember(x => x.Status, opt => opt.Ignore())
           .ForMember(x => x.Parent, opt => opt.Ignore())
           .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore())
           .ForMember(x => x.Id, opt => opt.Ignore())
           .ForMember(x => x.CreatedAt, opt => opt.Ignore())
           .ForMember(x => x.Category, opt => opt.Ignore())
           .ForMember(x => x.CategoryId, opt => opt.Ignore());

            CreateMap<AreaDto, Area>();
            CreateMap<PointOfInterestDto, PointOfInterest>();
            CreateMap<RouteDto, Route>();
            CreateMap<SegmentDto, Segment>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
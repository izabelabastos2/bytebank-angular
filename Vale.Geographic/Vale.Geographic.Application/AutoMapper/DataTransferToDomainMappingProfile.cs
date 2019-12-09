using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using GeoJSON.Net.Feature;
using AutoMapper;
using GeoAPI.Geometries;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {
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

            CreateMap<AreaDto, Area>()
              .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<PointOfInterestDto, PointOfInterest>()
              .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<RouteDto, Route>()
              .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<SegmentDto, Segment>()
              .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<CategoryDto, Category>();
        }
    }
}
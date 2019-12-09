using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using AutoMapper;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<PersonSample, PersonSampleDto>();
            CreateMap<Geometry, GeoJSON.Net.Geometry.MultiPolygon>();
            CreateMap<Geometry, GeoJSON.Net.Geometry.LineString>();
            CreateMap<Geometry, GeoJSON.Net.Geometry.Point>();


            CreateMap<Area, AreaDto>();
               //.ForMember(x => x.GeoJson.Properties["Name"], opt => opt.MapFrom(x => x.Name))
               //.ForMember(x => x.GeoJson.Properties["description"], opt => opt.MapFrom(x => x.Description))
               //.ForMember(x => x.GeoJson.Geometry, opt => opt.MapFrom(x => x.Location.AsText()));           

            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<Route, RouteDto>();
            CreateMap<Segment, SegmentDto>();
        }
    }
}
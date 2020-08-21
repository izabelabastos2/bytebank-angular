using GeoJSON.Net.Contrib.MsSqlSpatial;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using GeoJSON.Net.Feature;
using GeoAPI.Geometries;
using AutoMapper;
using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Application.Dto.Authorization;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<IGeometry, Feature>().ConvertUsing(x => new Feature(WktConvert.GeoJSONGeometry(x.ToString(), 4326), null, null));
            
            CreateMap<Auditory, AuditoryDto>();

            CreateMap<Category, CategoryDto>();

            CreateMap<User, UserDto>();


            CreateMap<FocalPoint, FocalPointDto>()
                .ForMember(dest => dest.Locality, opt => opt.Ignore())
                .ForMember(dest => dest.Answered, opt => opt.Ignore());

            CreateMap<Area, AreaDto>()
                .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location))
                .ForMember(dest => dest.CentralPoint, opt => opt.MapFrom(x => new Feature(WktConvert.GeoJSONGeometry(x.Location.Centroid.ToString(), 4326), null, null)));

            CreateMap<Area, PerimeterDto>()
                .ForMember(dest => dest.Sites, opt => opt.Ignore())
                .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));

            CreateMap<PointOfInterest, PointOfInterestDto>()
                .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location))
                .ForMember(dest => dest.Area, opt => opt.Ignore());            

            CreateMap<Route, RouteDto>()
                .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));

            CreateMap<Segment, SegmentDto>()
                .ForMember(dest => dest.Geojson, opt => opt.MapFrom(x => x.Location));

            CreateMap<NotificationAnswer, NotificationAnswerDto>();
        }
    }
}
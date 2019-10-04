using AutoMapper;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<PersonSample, PersonSampleDto>();

            CreateMap<Area, AreaDto>();
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<Route, RouteDto>();
            CreateMap<Segment, SegmentDto>();
        }
    }
}
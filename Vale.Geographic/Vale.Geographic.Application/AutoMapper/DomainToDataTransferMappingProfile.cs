using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using AutoMapper;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<PersonSample, PersonSampleDto>();

            CreateMap<Area, AreaDto>()
               .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<PointOfInterest, PointOfInterestDto>()
             .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<Route, RouteDto>()
              .ForMember(x => x.Location, opt => opt.Ignore());

            CreateMap<Segment, SegmentDto>()
              .ForMember(x => x.Location, opt => opt.Ignore());

        }
    }
}
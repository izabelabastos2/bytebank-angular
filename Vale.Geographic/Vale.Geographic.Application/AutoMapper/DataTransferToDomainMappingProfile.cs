using AutoMapper;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {
            CreateMap<AreaDto, Area>();
            CreateMap<PointOfInterestDto, PointOfInterest>();
            CreateMap<RouteDto, Route>();
            CreateMap<SegmentDto, Segment>();
        }
    }
}
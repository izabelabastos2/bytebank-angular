using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using GeoJSON.Net.Feature;
using System.Linq;
using AutoMapper;
using Vale.Geographic.Application.Dto.Authorization;
using Vale.Geographic.Domain.Entities.Authorization;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {
            CreateMap<Feature, Area>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(
                   x => x.Properties.Count == 0 || !x.Properties.Keys.Any(i => i.ToLower().Contains("name")) ? 
                    null : x.Properties.Where(y => y.Key.ToLower() == "name").Select(z => z.Value).FirstOrDefault()))

               .ForMember(dest => dest.Description, opt => opt.MapFrom(
                   x => x.Properties.Count == 0 || !x.Properties.Keys.Any(i => i.ToLower().Contains("description")) ?
                    null : x.Properties.Where(y => y.Key.ToLower() == "description").Select(z => z.Value).FirstOrDefault()))

               .ForMember(x => x.Location, opt => opt.Ignore())
               .ForMember(x => x.Color, opt => opt.Ignore())
               .ForMember(x => x.Status, opt => opt.Ignore())
               .ForMember(x => x.Parent, opt => opt.Ignore())
               .ForMember(x => x.ParentId, opt => opt.Ignore())
               .ForMember(x => x.CreatedAt, opt => opt.Ignore())
               .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore())
               .ForMember(x => x.CreatedBy, opt => opt.Ignore())
               .ForMember(x => x.LastUpdatedBy, opt => opt.Ignore())
               .ForMember(x => x.Id, opt => opt.Ignore())
               .ForMember(x => x.Category, opt => opt.Ignore())
               .ForMember(x => x.CategoryId, opt => opt.Ignore());


            CreateMap<AreaDto, Area>()
              .ForMember(x => x.Location, opt => opt.Ignore())
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());


            CreateMap<Feature, PointOfInterest>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(
                   x => x.Properties.Count == 0 || !x.Properties.Keys.Any(i => i.ToLower().Contains("name")) ?
                    null : x.Properties.Where(y => y.Key.ToLower() == "name").Select(z => z.Value).FirstOrDefault()))

               .ForMember(dest => dest.Description, opt => opt.MapFrom(
                   x => x.Properties.Count == 0 || !x.Properties.Keys.Any(i => i.ToLower().Contains("description")) ?
                    null : x.Properties.Where(y => y.Key.ToLower() == "description").Select(z => z.Value).FirstOrDefault()))

              .ForMember(x => x.Icon, opt => opt.Ignore())
              .ForMember(x => x.Location, opt => opt.Ignore())
              .ForMember(x => x.Status, opt => opt.Ignore())
              .ForMember(x => x.Area, opt => opt.Ignore())
              .ForMember(x => x.AreaId, opt => opt.Ignore())
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedBy, opt => opt.Ignore())
              .ForMember(x => x.Id, opt => opt.Ignore())
              .ForMember(x => x.Category, opt => opt.Ignore())
              .ForMember(x => x.CategoryId, opt => opt.Ignore());

            CreateMap<PointOfInterestDto, PointOfInterest>()
              .ForMember(x => x.Location, opt => opt.Ignore())
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());

            CreateMap<RouteDto, Route>()
              .ForMember(x => x.Location, opt => opt.Ignore())
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());

            CreateMap<SegmentDto, Segment>()
              .ForMember(x => x.Location, opt => opt.Ignore())
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());

            CreateMap<CategoryDto, Category>()
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());

            CreateMap<AuditoryDto, Auditory>()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Area, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.PointOfInterest, opt => opt.Ignore());

            CreateMap<FocalPointDto, FocalPoint>()
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());

            CreateMap<UserDto, User>()
              .ForMember(x => x.CreatedAt, opt => opt.Ignore())
              .ForMember(x => x.LastUpdatedAt, opt => opt.Ignore());
        }
    }
}
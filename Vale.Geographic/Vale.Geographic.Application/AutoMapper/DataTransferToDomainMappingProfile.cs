using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using GeoJSON.Net.Feature;
using AutoMapper;

namespace Vale.Geographic.Application.AutoMapper
{
    public class DataTransferToDomainMappingProfile : Profile
    {
        public DataTransferToDomainMappingProfile()
        {
            CreateMap<Feature, Area>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Properties.Count == 0 || !x.Properties.ContainsKey("Name") ? null : x.Properties["Name"]))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Properties.Count == 0 || !x.Properties.ContainsKey("Description") ? null : x.Properties["Description"]))
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
              .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Properties.Count == 0 || !x.Properties.ContainsKey("Name") ? null : x.Properties["Name"]))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Properties.Count == 0 || !x.Properties.ContainsKey("description") ? null : x.Properties["description"]))
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
        }
    }
}
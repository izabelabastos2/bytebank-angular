using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;
using System;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Application.Core.Services
{
    public class PerimeterAppService : AppService, IPerimeterAppService
    {
        private readonly IAreaRepository areaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISitesPerimetersRepository sitesPerimetersRepository;
        private readonly ISitesRepository sitesRepository;
        private readonly IAreaService areaService;
        private readonly IAuditoryService auditoryService;


        public PerimeterAppService(IUnitOfWork uoW,
                              IMapper mapper,
                              IAreaService areaService,
                              IAreaRepository areaRepository,
                              ICategoryRepository categoryRepository,
                              ISitesPerimetersRepository sitesPerimetersRepository,
                              ISitesRepository sitesRepository,
                              IAuditoryService auditoryService) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this.areaRepository = areaRepository;
            this.categoryRepository = categoryRepository;
            this.sitesPerimetersRepository = sitesPerimetersRepository;
            this.sitesRepository = sitesRepository;
            this.auditoryService = auditoryService;
        }


        public IEnumerable<PerimeterDto> GetAll(IFilterParameters parameters, Guid? filterBySiteId, out int total)
        {
            List<Guid> associatedPerimetersIds = new List<Guid>();
            Guid categoryId = GetOficialPerimeterCategoryId();

            if (filterBySiteId != null)
                associatedPerimetersIds = sitesPerimetersRepository
                    .GetAll(x => x.SiteId == filterBySiteId)
                    .Select(s => s.AreaId)
                    .ToList();

            IEnumerable<Area> perimeters = areaRepository
                .GetAll(
                    x => (associatedPerimetersIds.Count > 0 ? associatedPerimetersIds.Contains(x.Id) : true) && x.CategoryId == categoryId, 
                    parameters,
                    new string[] { "CategoryId", "Status" }
                )
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<PerimeterDto>>(perimeters);

        }

        public PerimeterDto GetById(Guid id)
        {
            Guid categoryId = GetOficialPerimeterCategoryId();
            Area perimeter = areaRepository.GetById(id);

            if (perimeter != null && perimeter.CategoryId != categoryId)
                return null;

            return Mapper.Map<PerimeterDto>(perimeter);
        }

        public PerimeterDto Insert(PerimeterDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Area perimeter = Mapper.Map<Area>(obj);

                perimeter.CategoryId = GetOficialPerimeterCategoryId();
                perimeter.LastUpdatedBy = perimeter.CreatedBy;

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                perimeter.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();

                Area createdPerimeter = areaService.Insert(perimeter);

                obj.Sites.ForEach(site =>
                {
                    sitesPerimetersRepository.Insert(new SitesPerimeter
                    {
                        AreaId = createdPerimeter.Id,
                        SiteId = site,
                        CreatedAt = obj.CreatedAt,
                        CreatedBy = obj.CreatedBy,
                        LastUpdatedAt = obj.LastUpdatedAt,
                        LastUpdatedBy = obj.LastUpdatedBy,
                        Status = obj.Status,
                    });
                });

                UoW.Commit();

                return Mapper.Map<PerimeterDto>(createdPerimeter);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        private Guid GetOficialPerimeterCategoryId()
        {
            var oficialPerimeterCategory = categoryRepository
                .GetAll(x => x.Status == true && x.TypeEntitie == TypeEntitieEnum.OficialPerimeter)
                .FirstOrDefault();

            if (oficialPerimeterCategory != null)
                return oficialPerimeterCategory.Id;
            else
                return new Guid();
        }
    }
}
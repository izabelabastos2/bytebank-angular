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
        public IAreaService areaService { get; set; }


        public PerimeterAppService(IUnitOfWork uoW,
                              IMapper mapper,
                              IAreaService areaService,
                              IAreaRepository areaRepository,
                              ICategoryRepository categoryRepository) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this.areaRepository = areaRepository;
            this.categoryRepository = categoryRepository;
        }


        public IEnumerable<PerimeterDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<Area> query = areaRepository
                .GetAll(x => true, parameters,
                    new string[] { "CategoryId", "Status", "ParentId" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<PerimeterDto>>(query);

        }

        public IEnumerable<AreaDto> Get(bool? active, Guid? id, Guid? categoryId, Guid? parentId, double? longitude, double? latitude, double? altitude, int? radiusDistance, DateTime? lastUpdatedAt, IFilterParameters request, out int total)
        {
            IGeometry point = null;

            if (longitude.HasValue && latitude.HasValue)
            {
                var coordenate = new GeoJSON.Net.Geometry.Point(new Position(latitude.Value, longitude.Value, altitude));
                var json = JsonConvert.SerializeObject(coordenate);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                point = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();
            }

            IEnumerable<Area> areas = areaRepository.Get(out total, id, null, point, active, categoryId, parentId, radiusDistance, lastUpdatedAt, request);

            return Mapper.Map<IEnumerable<AreaDto>>(areas);
        }

        public PerimeterDto Insert(PerimeterDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Area area = Mapper.Map<Area>(obj);

                area.CategoryId = GetOficialPerimeterCategoryId();

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                area.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();
                area.LastUpdatedBy = area.CreatedBy;

                area = areaService.Insert(area);

                UoW.Commit();

                return Mapper.Map<PerimeterDto>(area);
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
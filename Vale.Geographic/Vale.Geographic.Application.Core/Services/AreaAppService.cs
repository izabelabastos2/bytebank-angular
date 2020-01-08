using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
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

namespace Vale.Geographic.Application.Core.Services
{
    public class AreaAppService : AppService, IAreaAppService
    {
        private readonly IAreaRepository areaRepository;

        public AreaAppService(IUnitOfWork uoW, IMapper mapper, IAreaService areaService,
            IAreaRepository areaRepository) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this.areaRepository = areaRepository;
        }

        public IAreaService areaService { get; set; }

        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                Area area = areaService.GetById(id);

                if (area == null)
                    throw new ArgumentNullException();

                areaService.Delete(Mapper.Map<Area>(area));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public AreaDto GetById(Guid id)
        {
            return Mapper.Map<AreaDto>(areaService.GetById(id));
        }

        public IEnumerable<AreaDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<Area> query = areaRepository
                .GetAll(x => true, parameters,
                    new string[] { "CategoryId", "Status", "ParentId" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<AreaDto>>(query);

        }

        public IEnumerable<AreaDto> Get(bool? active, Guid? id, Guid? categoryId, Guid? parentId, double? longitude, double? latitude, double? altitude, IFilterParameters request, out int total)
        {
            IGeometry point = null;

            if (longitude.HasValue && latitude.HasValue)
            {
                var coordenate = new GeoJSON.Net.Geometry.Point(new Position(latitude.Value, longitude.Value, altitude));
                var json = JsonConvert.SerializeObject(coordenate);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                point = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));
            }

            IEnumerable<Area> areas = areaRepository.Get(id, out total, null, point, active, categoryId, parentId, request);

            return Mapper.Map<IEnumerable<AreaDto>>(areas);
        }

        public AreaDto Insert(AreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Area area = Mapper.Map<Area>(obj);

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);                
                area.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                area = areaService.Insert(area);               

                UoW.Commit();

                return Mapper.Map<AreaDto>(area);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public IEnumerable<AreaDto> Insert(CollectionAreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                List<Area> areas = new List<Area>();

                foreach (var feature in obj.Geojson.Features)
                {
                    Area area = Mapper.Map<Area>(feature);
                    area.CategoryId = obj.CategoryId;
                    area.ParentId = obj.ParentId;

                    var json = JsonConvert.SerializeObject(feature.Geometry);
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    area.Location = (Geometry) geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();
                           
                    areas.Add(areaService.Insert(area));                                  
                }

                UoW.Commit();

                return Mapper.Map<IEnumerable<AreaDto>>(areas);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public AreaDto Update(Guid id, AreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                //var areaOriginal = areaService.GetById(id);
                var areaOriginal = areaRepository.RecoverById(id);

                if (areaOriginal == null)
                    throw new ArgumentNullException();

                Area area = Mapper.Map<Area>(obj);
                area.Id = id;
                area.CreatedAt = areaOriginal.CreatedAt;

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                area.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                area = areaService.Update(area);

                UoW.Commit();

                return Mapper.Map<AreaDto>(area);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
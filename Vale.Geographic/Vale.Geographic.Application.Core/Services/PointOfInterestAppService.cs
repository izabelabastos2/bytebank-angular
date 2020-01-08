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
    public class PointOfInterestAppService : AppService, IPointOfInterestAppService
    {
        private readonly IPointOfInterestRepository pointOfInterestRepository;

        public PointOfInterestAppService(IUnitOfWork uoW, IMapper mapper, IPointOfInterestService pointOfInterestService,
            IPointOfInterestRepository pointOfInterestRepository) : base(uoW, mapper)
        {
            this.pointOfInterestService = pointOfInterestService;
            this.pointOfInterestRepository = pointOfInterestRepository;
        }

        public IPointOfInterestService pointOfInterestService { get; set; }

        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                PointOfInterest obj = pointOfInterestService.GetById(id);

                if (obj == null)
                    throw new ArgumentNullException();

                pointOfInterestService.Delete(Mapper.Map<PointOfInterest>(obj));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public PointOfInterestDto GetById(Guid id)
        {
            return Mapper.Map<PointOfInterestDto>(pointOfInterestService.GetById(id));
        }

        public IEnumerable<PointOfInterestDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<PointOfInterest> query = pointOfInterestRepository
                .GetAll(x => true, parameters,
                    new string[] { "CategoryId", "Status", "AreaId" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<PointOfInterestDto>>(query);
        }

        public IEnumerable<PointOfInterestDto> Get(bool? active, Guid? Id, Guid? categoryId, Guid? areaId, double? latitude, double? longitude,  double? altitude, IFilterParameters request, out int total)
        {
            IGeometry point = null;

            if (longitude.HasValue && latitude.HasValue)
            {
                var coordenate = new GeoJSON.Net.Geometry.Point(new Position(latitude.Value, longitude.Value, altitude));
                var json = JsonConvert.SerializeObject(coordenate);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                point = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));
            }

            IEnumerable<PointOfInterest> points = pointOfInterestRepository.Get( Id, out total, null, point, active, categoryId, areaId, request);

            return Mapper.Map<IEnumerable<PointOfInterestDto>>(points);
        }

        public PointOfInterestDto Insert(PointOfInterestDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                PointOfInterest pointOfInterest = Mapper.Map<PointOfInterest>(obj);

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                pointOfInterest.Location = (Geometry) geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                pointOfInterest = pointOfInterestService.Insert(pointOfInterest);

                UoW.Commit();

                return Mapper.Map<PointOfInterestDto>(pointOfInterest);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public IEnumerable<PointOfInterestDto> Insert(CollectionPointOfInterestDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                List<PointOfInterest> points = new List<PointOfInterest>();

                foreach (var feature in obj.Geojson.Features)
                {
                    PointOfInterest pointOfInterest = Mapper.Map<PointOfInterest>(feature);

                    pointOfInterest.AreaId = obj.AreaId;
                    pointOfInterest.CategoryId = obj.CategoryId;

                    var json = JsonConvert.SerializeObject(feature.Geometry);
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    pointOfInterest.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                    points.Add(pointOfInterestService.Insert(pointOfInterest));                       
                }                              

                UoW.Commit();

                return Mapper.Map<IEnumerable<PointOfInterestDto>>(points);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public PointOfInterestDto Update(Guid id, PointOfInterestDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                //var pointOfInterestOriginal = pointOfInterestService.GetById(id);
                var pointOfInterestOriginal = pointOfInterestRepository.RecoverById(id);            
                if (pointOfInterestOriginal == null)
                    throw new ArgumentNullException();

                PointOfInterest pointOfInterest = Mapper.Map<PointOfInterest>(obj);
                pointOfInterest.Id = id;
                pointOfInterest.CreatedAt = pointOfInterestOriginal.CreatedAt;

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                pointOfInterest.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                pointOfInterest = pointOfInterestService.Update(pointOfInterest);

                UoW.Commit();

                return Mapper.Map<PointOfInterestDto>(pointOfInterest);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
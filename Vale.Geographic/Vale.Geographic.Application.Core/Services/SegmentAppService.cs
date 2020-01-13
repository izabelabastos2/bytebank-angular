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
using NetTopologySuite.IO;
using NetTopologySuite;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;

namespace Vale.Geographic.Application.Core.Services
{
    public class SegmentAppService : AppService, ISegmentAppService
    {
        private readonly ISegmentRepository segmentRepository;

        public SegmentAppService(IUnitOfWork uoW, IMapper mapper, ISegmentService segmentService,
            ISegmentRepository segmentRepository) : base(uoW, mapper)
        {
            this.segmentService = segmentService;
            this.segmentRepository = segmentRepository;
        }

        public ISegmentService segmentService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                Segment segment = segmentService.GetById(id);

                if (segment == null)
                    throw new ArgumentNullException();

                segmentService.Delete(Mapper.Map<Segment>(segment));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
        
        public SegmentDto GetById(Guid id)
        {
            return Mapper.Map<SegmentDto>(segmentService.GetById(id));
        }

        public IEnumerable<SegmentDto> GetAll(IFilterParameters parameters, out int total)
        {

            var query = segmentRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<SegmentDto>>(query);

        }

        public IEnumerable<SegmentDto> Get(bool? active, Guid? id, Guid? areaId, Guid? routeId, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<SegmentDto>>(segmentRepository.Get(id, out total, active, areaId, routeId, request));
        }

        public SegmentDto Insert(SegmentDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Segment segment = Mapper.Map<Segment>(obj);

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                segment.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

                segment = segmentService.Insert(segment);

                UoW.Commit();

                return Mapper.Map<SegmentDto>(segment);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public SegmentDto Update(Guid id, SegmentDto obj)
        {
            try
            {
                UoW.BeginTransaction();


                var segmentOriginal = segmentService.GetById(id);
                UoW.Context.Entry(segmentOriginal).State = EntityState.Detached;

                if (segmentOriginal == null)
                    throw new ArgumentNullException();

                Segment segment = Mapper.Map<Segment>(obj);
                segment.Id = id;
                segment.CreatedAt = segmentOriginal.CreatedAt;

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                segment.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));
                segment = segmentService.Update(segment);

                UoW.Commit();

                return Mapper.Map<SegmentDto>(segment);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
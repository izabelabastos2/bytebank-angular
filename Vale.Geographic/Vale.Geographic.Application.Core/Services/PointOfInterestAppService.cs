using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Application.Core.Services
{
    public class PointOfInterestAppService : AppService, IPointOfInterestAppService
    {
        private readonly IPointOfInterestRepository _pointOfInterestRepository;

        public PointOfInterestAppService(IUnitOfWork uoW, IMapper mapper, IPointOfInterestService pointOfInterestService,
            IPointOfInterestRepository pointOfInterestRepository) : base(uoW, mapper)
        {
            this.pointOfInterestService = pointOfInterestService;
            this._pointOfInterestRepository = pointOfInterestRepository;
        }

        public IPointOfInterestService pointOfInterestService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = pointOfInterestService.GetById(id);
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

            var query = _pointOfInterestRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<PointOfInterestDto>>(query);

        }

        public IEnumerable<PointOfInterestDto> Get(bool? active, Guid categoryId, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<PointOfInterestDto>>(_pointOfInterestRepository.Get(active, categoryId, request.sort,
                request.page, request.per_page, out total));
        }

        public PointOfInterestDto Insert(PointOfInterestDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                PointOfInterest pointOfInterest = Mapper.Map<PointOfInterest>(obj);

                var json = JsonConvert.SerializeObject(obj.Location);
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

        public PointOfInterestDto Update(Guid id, PointOfInterestDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var PointOfInterest = Mapper.Map<Domain.Entities.PointOfInterest>(obj);
                PointOfInterest.Id = id;
                PointOfInterest = pointOfInterestService.Update(PointOfInterest);

                UoW.Commit();
                return Mapper.Map<PointOfInterestDto>(PointOfInterest);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
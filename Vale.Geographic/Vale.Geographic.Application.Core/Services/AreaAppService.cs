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

namespace Vale.Geographic.Application.Core.Services
{
    public class AreaAppService : AppService, IAreaAppService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaAppService(IUnitOfWork uoW, IMapper mapper, IAreaService areaService,
            IAreaRepository areaRepository) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this._areaRepository = areaRepository;
        }

        public IAreaService areaService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = areaService.GetById(id);
                areaService.Delete(Mapper.Map<Area>(obj));
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

            var query = _areaRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<AreaDto>>(query);

        }

        public IEnumerable<AreaDto> Get(bool? active, Guid categoryId, GeoJSON.Net.Geometry.Point location, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<AreaDto>>(_areaRepository.Get(active, categoryId, request.sort,
                request.page, request.per_page, out total));
        }

        public AreaDto Insert(AreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Area area = Mapper.Map<Area>(obj);

                var json = JsonConvert.SerializeObject(obj.Location);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                area.Location = geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

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

        public IEnumerable<AreaDto> Insert(GeoJsonDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                List<Area> areas = new List<Area>();

                foreach (var feature in obj.FeatureCollection.Features)
                {
                    var area = Mapper.Map<Area>(feature);

                    area.CategoryId = obj.CategoryId;

                    var json = JsonConvert.SerializeObject(feature.Geometry);
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    area.Location = (Geometry) geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));                    

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

                var Area = Mapper.Map<Area>(obj);
                Area.Id = id;
                Area = areaService.Update(Area);

                UoW.Commit();
                return Mapper.Map<AreaDto>(Area);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
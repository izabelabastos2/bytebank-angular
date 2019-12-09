using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoJSON.Net.Geometry;
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
    public class RouteAppService : AppService, IRouteAppService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteAppService(IUnitOfWork uoW, IMapper mapper, IRouteService routeService,
            IRouteRepository routeRepository) : base(uoW, mapper)
        {
            this.routeService = routeService;
            this._routeRepository = routeRepository;
        }

        public IRouteService routeService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = routeService.GetById(id);
                routeService.Delete(Mapper.Map<Route>(obj));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }


        public RouteDto GetById(Guid id)
        {
            return Mapper.Map<RouteDto>(routeService.GetById(id));
        }

        public IEnumerable<RouteDto> GetAll(IFilterParameters parameters, out int total)
        {

            var query = _routeRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<RouteDto>>(query);

        }

        public IEnumerable<RouteDto> Get(bool? active, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<RouteDto>>(_routeRepository.Get(active, request.sort,
                request.page, request.per_page, out total));
        }

        public RouteDto Insert(RouteDto obj)
        {
            try
            {
                UoW.BeginTransaction();
                var Thing = routeService.Insert(Mapper.Map<Route>(obj));

                Route route = Mapper.Map<Route>(obj);

                var json = JsonConvert.SerializeObject(obj.Location);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                route.Location = (Geometry) geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));
                route = routeService.Insert(route);

                UoW.Commit();

                return Mapper.Map<RouteDto>(Thing);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public RouteDto Update(Guid id, RouteDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var Route = Mapper.Map<Route>(obj);
                Route.Id = id;
                Route = routeService.Update(Route);

                UoW.Commit();
                return Mapper.Map<RouteDto>(Route);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
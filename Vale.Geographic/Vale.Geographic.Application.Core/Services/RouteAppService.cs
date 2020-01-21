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
    public class RouteAppService : AppService, IRouteAppService
    {
        private readonly IRouteRepository routeRepository;

        public RouteAppService(IUnitOfWork uoW, IMapper mapper, IRouteService routeService,
            IRouteRepository routeRepository) : base(uoW, mapper)
        {
            this.routeService = routeService;
            this.routeRepository = routeRepository;
        }

        public IRouteService routeService { get; set; }
        
        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                Route route = routeService.GetById(id);

                if (route == null)
                    throw new ArgumentNullException();

                routeService.Delete(Mapper.Map<Route>(route));
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

            IEnumerable<Route> query = routeRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<RouteDto>>(query);

        }

        public IEnumerable<RouteDto> Get(bool? active, Guid? id, Guid? areaId, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<RouteDto>>(routeRepository.Get(id, out total, active, areaId, request));
        }

        public RouteDto Insert(RouteDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                Route route = Mapper.Map<Route>(obj);

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                route.Location = (Geometry) geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();
                route = routeService.Insert(route);

                UoW.Commit();

                return Mapper.Map<RouteDto>(route);
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

                var routeOriginal = routeService.GetById(id);
                UoW.Context.Entry(routeOriginal).State = EntityState.Detached;

                if (routeOriginal == null)
                    throw new ArgumentNullException();

                Route route = Mapper.Map<Route>(obj);
                route.Id = id;
                route.CreatedAt = routeOriginal.CreatedAt;

                var json = JsonConvert.SerializeObject(obj.Geojson.Geometry);
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                route.Location = (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();

                route = routeService.Update(route);

                UoW.Commit();

                return Mapper.Map<RouteDto>(route);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
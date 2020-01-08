using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IRouteAppService
    {
        IRouteService routeService { get; set; }

        void Delete(Guid id);
        RouteDto GetById(Guid id);
        RouteDto Insert(RouteDto obj);
        RouteDto Update(Guid id, RouteDto obj);
        IEnumerable<RouteDto> Get(bool? active, Guid? id, Guid? areaId, IFilterParameters request, out int total);
        IEnumerable<RouteDto> GetAll(IFilterParameters parameters, out int total);
    }
}
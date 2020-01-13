using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IRouteRepository : IRepository<Route>
    {
        IEnumerable<Route> Get(Guid? id, out int total, bool? active = null, Guid? areaId = null, IFilterParameters parameters = null);
    }
}
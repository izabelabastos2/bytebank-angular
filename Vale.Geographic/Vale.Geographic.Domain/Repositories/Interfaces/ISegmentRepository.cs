using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface ISegmentRepository : IRepository<Segment>
    {
        IEnumerable<Segment> Get(Guid? id, out int total, bool? active = null, Guid? areaId = null, Guid? routeId = null, IFilterParameters parameters = null);

    }
}
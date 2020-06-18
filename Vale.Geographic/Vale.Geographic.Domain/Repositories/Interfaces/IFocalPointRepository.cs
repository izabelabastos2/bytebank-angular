using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IFocalPointRepository : IRepository<FocalPoint>
    {
        IEnumerable<FocalPoint> Get(out int total, bool? active, string matricula, Guid? localityId = null, Guid? pointOfInterestId = null, IFilterParameters parameters = null);
    }
}

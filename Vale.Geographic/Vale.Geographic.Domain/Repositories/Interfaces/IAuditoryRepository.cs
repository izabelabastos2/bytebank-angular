using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IAuditoryRepository : IRepository<Auditory>
    {
        IEnumerable<Auditory> Get(Guid? areaId, Guid? pointOfInterestId, Guid? categoryId, TypeEntitieEnum? typeEntitie, out int total, IFilterParameters parameters = null);
    }
}

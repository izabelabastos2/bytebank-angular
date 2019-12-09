using System;
using System.Collections.Generic;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IPointOfInterestRepository : IRepository<PointOfInterest>
    {
        IEnumerable<PointOfInterest> Get(bool? active, Guid categoryId, string sort, int page, int per_page, out int total);
    }
}
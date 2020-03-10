using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using GeoAPI.Geometries;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IPointOfInterestRepository : IRepository<PointOfInterest>
    {
        IEnumerable<PointOfInterest> Get(out int total, Guid? Id = null, IGeometry location = null, IGeometry point = null, bool? active = null, Guid? categoryId = null, Guid? areaId = null, int? radiusDistance = null, DateTime? lastUpdatedAt = null, IFilterParameters parameters = null);
    }
}
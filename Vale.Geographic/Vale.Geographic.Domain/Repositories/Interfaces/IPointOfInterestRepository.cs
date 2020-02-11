using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using GeoAPI.Geometries;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IPointOfInterestRepository : IRepository<PointOfInterest>
    {
        IEnumerable<PointOfInterest> Get(Guid? Id, out int total, IGeometry location = null, IGeometry point = null, bool? active = null, Guid? categoryId = null, Guid? areaId = null, int? radiusDistance = null, IFilterParameters parameters = null);
    }
}
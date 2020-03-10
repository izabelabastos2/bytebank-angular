using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using GeoAPI.Geometries;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IAreaRepository : IRepository<Area>
    {
        IEnumerable<Area> Get(out int total, Guid? id = null, IGeometry location = null, IGeometry point = null, bool? active = null, Guid? categoryId = null, Guid? parentId = null, int? radiusDistance = null, DateTime? lastUpdatedAt = null, IFilterParameters parameters = null);
    }
}
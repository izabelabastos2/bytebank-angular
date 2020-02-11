using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IPointOfInterestAppService
    {
        IPointOfInterestService pointOfInterestService { get; set; }

        void Delete(Guid id);
        PointOfInterestDto GetById(Guid id);
        PointOfInterestDto Insert(PointOfInterestDto obj);
        IEnumerable<PointOfInterestDto> Insert(CollectionPointOfInterestDto GeoJson);
        PointOfInterestDto Update(Guid id, PointOfInterestDto obj);
        IEnumerable<PointOfInterestDto> Get(bool? active, Guid? Id, Guid? categoryId, Guid? areaId, double? longitude, double? latitude, double? altitude, int? radiusDistance, IFilterParameters request, out int total);
        IEnumerable<PointOfInterestDto> GetAll(IFilterParameters parameters, out int total);
    }
}
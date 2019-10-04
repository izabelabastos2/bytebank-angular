using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface IPointOfInterestAppService
    {
        IPointOfInterestService pointOfInterestService { get; set; }

        void Delete(Guid id);
        PointOfInterestDto GetById(Guid id);
        PointOfInterestDto Insert(PointOfInterestDto obj);
        PointOfInterestDto Update(Guid id, PointOfInterestDto obj);
        IEnumerable<PointOfInterestDto> Get(bool? active, IFilterParameters request, out int total);
        IEnumerable<PointOfInterestDto> GetAll(IFilterParameters parameters, out int total);
    }
}
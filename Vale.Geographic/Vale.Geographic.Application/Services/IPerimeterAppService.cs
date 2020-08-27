using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IPerimeterAppService
    {
        IEnumerable<PerimeterDto> GetAll(IFilterParameters defaultParameters, Guid? filterBySiteId, out int total);
        PerimeterDto GetById(Guid id);
        PerimeterDto Insert(PerimeterDto obj);
        PerimeterDto Update(PerimeterDto obj);
        bool Delete(Guid id, string updatedBy);
    }
}
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IAreaAppService
    {
        IAreaService areaService { get; set; }

        void Delete(Guid id, string lastUpdatedBy);
        AreaDto GetById(Guid id);
        AreaDto Insert(AreaDto obj);
        AreaDto Update(Guid id, AreaDto obj);
        IEnumerable<AreaDto> Insert(CollectionAreaDto obj);
        IEnumerable<AreaDto> GetAll(IFilterParameters parameters, out int total);
        IEnumerable<AreaDto> Get(bool? active, Guid? id, Guid? categoryId, Guid? parentId, double? longitude, double? latitude, double? altitude, int? radiusDistance, DateTime? lastUpdatedAt, IFilterParameters request, out int total);
    }
}
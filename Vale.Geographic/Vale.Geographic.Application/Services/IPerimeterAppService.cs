using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IPerimeterAppService
    {
        IAreaService areaService { get; set; }

        IEnumerable<PerimeterDto> GetAll(IFilterParameters parameters, out int total);
        IEnumerable<AreaDto> Get(bool? active, Guid? id, Guid? categoryId, Guid? parentId, double? longitude, double? latitude, double? altitude, int? radiusDistance, DateTime? lastUpdatedAt, IFilterParameters request, out int total);
        PerimeterDto Insert(PerimeterDto obj);
    }
}
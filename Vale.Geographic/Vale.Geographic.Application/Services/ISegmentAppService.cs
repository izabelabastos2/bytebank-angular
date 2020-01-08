using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface ISegmentAppService
    {
        ISegmentService segmentService { get; set; }

        void Delete(Guid id);
        SegmentDto GetById(Guid id);
        SegmentDto Insert(SegmentDto obj);
        SegmentDto Update(Guid id, SegmentDto obj);
        IEnumerable<SegmentDto> Get(bool? active, Guid? id, Guid? areaId, Guid? routeId, IFilterParameters request, out int total);
        IEnumerable<SegmentDto> GetAll(IFilterParameters parameters, out int total);
    }
}
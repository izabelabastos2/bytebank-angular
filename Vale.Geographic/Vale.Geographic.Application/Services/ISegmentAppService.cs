using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface ISegmentAppService
    {
        ISegmentService segmentService { get; set; }

        void Delete(Guid id);
        SegmentDto GetById(Guid id);
        SegmentDto Insert(SegmentDto obj);
        SegmentDto Update(Guid id, SegmentDto obj);
        IEnumerable<SegmentDto> Get(bool? active, IFilterParameters request, out int total);
        IEnumerable<SegmentDto> GetAll(IFilterParameters parameters, out int total);
    }
}
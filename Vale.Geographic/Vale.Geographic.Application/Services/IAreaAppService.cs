using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface IAreaAppService
    {
        IAreaService areaService { get; set; }

        void Delete(Guid id);
        AreaDto GetById(Guid id);
        AreaDto Insert(AreaDto obj);
        AreaDto Update(Guid id, AreaDto obj);
        IEnumerable<AreaDto> Get(bool? active, IFilterParameters request, out int total);
        IEnumerable<AreaDto> GetAll(IFilterParameters parameters, out int total);
    }
}
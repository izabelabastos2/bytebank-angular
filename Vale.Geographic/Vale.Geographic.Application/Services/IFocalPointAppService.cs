using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface IFocalPointAppService
    {
        IFocalPointService focalPointService { get; set; }

        void Delete(Guid id, string lastUpdatedBy);
        FocalPointDto GetById(Guid id);
        FocalPointDto GetByMatricula(string matricula);
        FocalPointDto Insert(FocalPointDto obj);
        FocalPointDto Update(Guid id, FocalPointDto obj);
        IEnumerable<FocalPointDto> GetAll(IFilterParameters parameters, out int total);
        IEnumerable<FocalPointDto> Get(bool? active, Guid? localityId, Guid? pointOfInterestId, string matricula, IFilterParameters parameters, out int total);
    }
}

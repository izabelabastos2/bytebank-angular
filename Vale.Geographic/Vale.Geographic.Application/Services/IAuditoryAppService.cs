using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Application.Services
{
    public interface IAuditoryAppService
    {
        IAuditoryService auditoryService { get; set; }
        IEnumerable<AuditoryDto> GetAll(IFilterParameters parameters, out int total);
        AuditoryDto GetById(Guid id);
        IEnumerable<AuditoryDto> Get(Guid? areaId, Guid? pointOfInterestId, Guid? categoryId, TypeEntitieEnum? typeEntitie, IFilterParameters parameters, out int total);

    }
}

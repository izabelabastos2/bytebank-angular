using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System;

namespace Vale.Geographic.Application.Core.Services
{
    public class AuditoryAppService : AppService, IAuditoryAppService
    {
        private readonly IAuditoryRepository auditoryRepository;
        public IAuditoryService auditoryService { get; set; }

        public AuditoryAppService(IUnitOfWork uoW, 
                                  IMapper mapper, 
                                  IAuditoryService auditoryService,
                                  IAuditoryRepository auditoryRepository) : base(uoW, mapper)
        {
            this.auditoryService = auditoryService;
            this.auditoryRepository = auditoryRepository;
        }

        public IEnumerable<AuditoryDto> Get(Guid? areaId, Guid? pointOfInterestId, Guid? categoryId, TypeEntitieEnum? typeEntitie, IFilterParameters parameters, out int total)
        {
            IEnumerable<Auditory> auditorys = auditoryRepository.Get(areaId, pointOfInterestId, categoryId, typeEntitie, out total, parameters);

            return Mapper.Map<IEnumerable<AuditoryDto>>(auditorys);
        }

        public IEnumerable<AuditoryDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<Auditory> query = auditoryRepository
                .GetAll(x => true, parameters,
                    new string[] { "CategoryId", "Status", "ParentId" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<AuditoryDto>>(query);
        }

        public AuditoryDto GetById(Guid id)
        {
            return Mapper.Map<AuditoryDto>(auditoryService.GetById(id));
        }
    }
}

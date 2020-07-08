using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using NetTopologySuite.IO;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Services
{
    public class AreaService : Service<Area>, IAreaService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IAuditoryService auditoryService;

        public AreaService(IUnitOfWork context, 
                           IAreaRepository rep, 
                           ICategoryRepository categoryRepository,
                           IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new AreaValidator(rep, categoryRepository);
            this.categoryRepository = categoryRepository;
            this.auditoryService = auditoryService;
        }

        public override Area Insert(Area obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if(obj.CategoryId.HasValue)
                 obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override Area Update(Area obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (obj.CategoryId.HasValue)
                obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(Area newObj, Area oldObj )
        {
            var audit = new Auditory();
            audit.AreaId = newObj.Id;
            audit.TypeEntitie = Enumerable.TypeEntitieEnum.Area;
            audit.CreatedBy = newObj.LastUpdatedBy;
            audit.LastUpdatedBy = newObj.LastUpdatedBy;
            audit.Status = true;


            if (!newObj.Equals(oldObj))
            {
                var json = GeoJsonSerializer.Create();
                var sw = new System.IO.StringWriter();

                json.Serialize(sw, newObj);
                audit.NewValue = sw.ToString();

                sw = new System.IO.StringWriter();
                json.Serialize(sw, oldObj);

                audit.OldValue = sw.ToString();

                auditoryService.Insert(audit);
            }

        }


    }
}
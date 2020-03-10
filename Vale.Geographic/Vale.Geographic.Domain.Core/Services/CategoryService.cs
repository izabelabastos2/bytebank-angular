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
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IAuditoryService auditoryService;

        public CategoryService(IUnitOfWork context, 
                               ICategoryRepository rep,
                               IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new CategoryValidator(rep);
            this.auditoryService = auditoryService;
        }

        public override Category Insert(Category obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override Category Update(Category obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(Category newObj, Category oldObj)
        {
            var Audit = new Auditory();
            Audit.CategoryId = newObj.Id;
            Audit.TypeEntitie = Enumerable.TypeEntitieEnum.Category;
            Audit.CreatedBy = newObj.LastUpdatedBy;
            Audit.LastUpdatedBy = newObj.LastUpdatedBy;
            Audit.Status = true;


            if (!newObj.Equals(oldObj))
            {
                var json = GeoJsonSerializer.Create();
                var sw = new System.IO.StringWriter();

                json.Serialize(sw, newObj);
                Audit.NewValue = sw.ToString();

                sw = new System.IO.StringWriter();

                json.Serialize(sw, oldObj);
                Audit.OldValue = sw.ToString();

                auditoryService.Insert(Audit);
            }
        }
    }   
}

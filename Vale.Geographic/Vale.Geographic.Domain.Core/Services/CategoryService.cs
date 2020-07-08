using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using NetTopologySuite.IO;
using FluentValidation;
using System;
using Newtonsoft.Json;

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
            var audit = new Auditory();
            audit.CategoryId = newObj.Id;
            audit.TypeEntitie = Enumerable.TypeEntitieEnum.Category;
            audit.CreatedBy = newObj.LastUpdatedBy;
            audit.LastUpdatedBy = newObj.LastUpdatedBy;
            audit.Status = true;


            if (!newObj.Equals(oldObj))
            {
                audit.NewValue = JsonConvert.SerializeObject(newObj).ToString();

                audit.OldValue = JsonConvert.SerializeObject(oldObj).ToString();

                auditoryService.Insert(audit);
            }
        }
    }   
}

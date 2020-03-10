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
    public class PointOfInterestService : Service<PointOfInterest>, IPointOfInterestService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IAuditoryService auditoryService;

        public PointOfInterestService(IUnitOfWork context, 
                                      IPointOfInterestRepository rep, 
                                      IAreaRepository areaRepository, 
                                      ICategoryRepository categoryRepository,
                                      IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new  PointOfInterestValidator(rep, areaRepository, categoryRepository);
            this.categoryRepository = categoryRepository;
            this.auditoryService = auditoryService;
        }

        public override PointOfInterest Insert(PointOfInterest obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (obj.CategoryId.HasValue)
                obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override PointOfInterest Update(PointOfInterest obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (obj.CategoryId.HasValue)
                obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(PointOfInterest newObj, PointOfInterest oldObj)
        {
            var Audit = new Auditory();
            Audit.PointOfInterestId = newObj.Id;
            Audit.TypeEntitie = Enumerable.TypeEntitieEnum.PointOfInterest;
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
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
    public class FocalPointService : Service<FocalPoint>, IFocalPointService
    {
        private readonly IAuditoryService auditoryService;

        public FocalPointService(IUnitOfWork context,
                                 IFocalPointRepository rep,
                                 IAuditoryService auditoryService,
                                 IPointOfInterestRepository pointOfInterestRepository,
                                 IAreaRepository areaRepository) : base(context, rep)
        {
            Validator = new FocalPointValidator(rep, pointOfInterestRepository, areaRepository);
            this.auditoryService = auditoryService;
        }

        public override FocalPoint Insert(FocalPoint obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override FocalPoint Update(FocalPoint obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(FocalPoint newObj, FocalPoint oldObj)
        {
            var audit = new Auditory();
            audit.TypeEntitie = Enumerable.TypeEntitieEnum.FocalPoint;
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

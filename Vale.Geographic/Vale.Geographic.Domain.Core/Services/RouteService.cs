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
    public class RouteService : Service<Route>, IRouteService
    {
        private readonly IAuditoryService auditoryService;

        public RouteService(IUnitOfWork context, 
                            IRouteRepository rep, 
                            IAreaRepository areaRepository,
                            IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new RouteValidator(rep, areaRepository);
            this.auditoryService = auditoryService;
        }

        public override Route Insert(Route obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override Route Update(Route obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(Route newObj, Route oldObj)
        {
            var audit = new Auditory();
            audit.TypeEntitie = Enumerable.TypeEntitieEnum.Route;
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
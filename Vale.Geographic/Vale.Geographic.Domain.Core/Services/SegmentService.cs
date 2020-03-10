﻿using Vale.Geographic.Domain.Repositories.Interfaces;
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
    public class SegmentService : Service<Segment>, ISegmentService
    {
        private readonly IAuditoryService auditoryService;

        public SegmentService(IUnitOfWork context, 
                              ISegmentRepository rep, 
                              IRouteRepository routeRepository, 
                              IAreaRepository areaRepository,
                              IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new SegmentValidator(rep, routeRepository, areaRepository);
            this.auditoryService = auditoryService;
        }

        public override Segment Insert(Segment obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override Segment Update(Segment obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(Segment newObj, Segment oldObj)
        {
            var Audit = new Auditory();
            Audit.TypeEntitie = Enumerable.TypeEntitieEnum.Segment;
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
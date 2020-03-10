using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Services
{
    public class AuditoryService : Service<Auditory>, IAuditoryService
    {
        public AuditoryService(IUnitOfWork context, IAuditoryRepository rep) : base(context, rep)
        {
            Validator = new AuditoryValidator(rep);
        }

        public override Auditory Insert(Auditory obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override Auditory Update(Auditory obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }
    }
}

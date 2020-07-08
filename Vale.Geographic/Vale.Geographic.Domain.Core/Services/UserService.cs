using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using NetTopologySuite.IO;
using FluentValidation;
using System;
using Newtonsoft.Json;

namespace Vale.Geographic.Domain.Core.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IAuditoryService auditoryService;

        public UserService(IUnitOfWork context,
                               IUserRepository rep,
                               IAuditoryService auditoryService) : base(context, rep)
        {
            Validator = new UserValidator(rep);
            this.auditoryService = auditoryService;
        }

        public override User Insert(User obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override User Update(User obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }

        public void InsertAuditory(User newObj, User oldObj)
        {
            var audit = new Auditory();
            audit.TypeEntitie = Enumerable.TypeEntitieEnum.User;
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

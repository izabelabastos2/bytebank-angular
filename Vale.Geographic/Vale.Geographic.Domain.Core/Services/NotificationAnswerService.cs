using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Domain.Core.Services
{
    public class NotificationAnswerService : Service<NotificationAnswer>, INotificationAnswerService
    {
        private readonly IFocalPointRepository focalPointRepository;
        public NotificationAnswerService(IUnitOfWork context,
                                  INotificationAnswerRepository rep,                                 
                                 IFocalPointRepository focalPointRepository
            ) : base(context, rep)
        {
            Validator = new NotificationAnswerValidator(rep);
            this.focalPointRepository = focalPointRepository;
        }
        public override NotificationAnswer Insert(NotificationAnswer obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;            

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override NotificationAnswer Update(NotificationAnswer obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;
            FocalPoint focalPoint = focalPointRepository.GetById(obj.FocalPointId);            
            obj.LastUpdatedBy = focalPoint.Matricula;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Answered = true;

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }
    }
}

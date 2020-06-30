using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class NotificationAnswerValidator : AbstractValidator<NotificationAnswer>
    {
        private readonly INotificationAnswerRepository notificationAnswerRepository;

        public NotificationAnswerValidator(INotificationAnswerRepository notificationAnswerRepository)
        {
            ValidateId();
            ValidateFocalPointId();
            ValidateNotificationId();
            ValidateAnswered();            

            RuleSet("Insert", () =>
            {
                ValidateFocalPointId();
                ValidateNotificationId();
                ValidateAnswered();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateFocalPointId();
                ValidateNotificationId();
                ValidateAnswered();
            });

            this.notificationAnswerRepository = notificationAnswerRepository;
        }

        #region Validação do dados

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.NotificationAnswerIdRequired);
        }

        private void ValidateFocalPointId()
        {
            RuleFor(o => o.FocalPointId)
                .NotEmpty().WithMessage(Resources.Validations.NotificationAnswerFocalPointIdRequired);
        }

        private void ValidateNotificationId()
        {
            RuleFor(o => o.NotificationId)
                .NotEmpty().WithMessage(Resources.Validations.NotificationAnswerNotificationIdRequired);
        }

        private void ValidateAnswered()
        {
            RuleFor(o => o.Answered)
                .NotNull().WithMessage(Resources.Validations.NotificationAnswerAnsweredRequired);
        }
        #endregion
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Repositories.Interfaces;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserRepository userRepository;

        public UserValidator(IUserRepository userRepository)
        {
            ValidateId();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateCreatedBy();
            ValidateLastUpdatedBy();
            ValidateStatus();
            ValidateName();
            ValidateEmail();
            ValidateMatricula();
            ValidateProfile();

            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
                ValidateStatus();
                ValidateEmail();
                ValidateMatricula();
                ValidateProfile();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateName();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
                ValidateStatus();
                ValidateEmail();
                ValidateMatricula();
                ValidateProfile();

            });

            this.userRepository = userRepository;
        }

        #region Validação do dados

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.UserIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.UserNameRequired)
                .Length(1, 255).WithMessage(Resources.Validations.UserNameLength);
        }

        private void ValidateEmail()
        {
            RuleFor(o => o.Email)
                .Length(1, 100).WithMessage(Resources.Validations.UserEmailLength);
        }

        private void ValidateMatricula()
        {
            RuleFor(o => o.Matricula)
                .NotEmpty().WithMessage(Resources.Validations.UserMatriculaRequired)
                .Length(1, 15).WithMessage(Resources.Validations.UserMatriculaLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.UserCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.UserLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.UserStatusRequired);
        }

        private void ValidateCreatedBy()
        {
            RuleFor(o => o.CreatedBy)
                .NotEmpty().WithMessage(Resources.Validations.UserCreatedByRequired);
        }

        private void ValidateLastUpdatedBy()
        {
            RuleFor(o => o.LastUpdatedBy)
                .NotEmpty().WithMessage(Resources.Validations.UserLastUpdatedByRequired);
        }

        private void ValidateProfile()
        {
            RuleFor(o => o.Profile)
                .NotEmpty().WithMessage(Resources.Validations.UserProfileRequired);
        }
        
        #endregion

    }
}

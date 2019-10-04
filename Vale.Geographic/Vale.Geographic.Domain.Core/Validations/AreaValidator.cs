using FluentValidation;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class AreaValidator : AbstractValidator<Area>
    {
        public AreaValidator()
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();


            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
            });

        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id).NotEmpty().WithMessage(Resources.Validations.PersonSampleIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleFirstNameRequired)
                .Length(1, 50).WithMessage(Resources.Validations.PersonSampleFirstNameLength);
        }

        private void ValidateDescription()
        {
            RuleFor(o => o.Description)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleLastNameRequired)
                .Length(1, 50).WithMessage(Resources.Validations.PersonSampleLastNameLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleDateBirthRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleDateBirthRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotEmpty().WithMessage(Resources.Validations.PersonSampleTypeRequired);
        }

        #endregion Validações de campos
    }
}
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities;
using FluentValidation;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class AuditoryValidator : AbstractValidator<Auditory>
    {
        private readonly IAuditoryRepository auditoryRepository;

        public AuditoryValidator(IAuditoryRepository auditoryRepository)
        {
            this.auditoryRepository = auditoryRepository;

            ValidateId();
            ValidateOldValue();
            ValidateType();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();
            ValidateCreatedBy();
            ValidateLastUpdatedBy();

            RuleSet("Insert", () =>
            {
                ValidateOldValue();
                ValidateType();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();

            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateOldValue();
                ValidateType();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
            });
        }

        #region Validação do dados

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryIdRequired);
        }

        private void ValidateOldValue()
        {
            RuleFor(o => o.OldValue)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryOldValueRequired);
        }

        private void ValidateNewValue()
        {
            RuleFor(o => o.NewValue)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryNewValueRequired);
        }

        private void ValidateType()
        {
            RuleFor(o => o.TypeEntitie)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryTypeRequired);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.AuditoryStatusRequired);
        }

        private void ValidateCreatedBy()
        {
            RuleFor(o => o.CreatedBy)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryCreatedByRequired);
        }

        private void ValidateLastUpdatedBy()
        {
            RuleFor(o => o.LastUpdatedBy)
                .NotEmpty().WithMessage(Resources.Validations.AuditoryLastUpdatedByRequired);
        }

        #endregion

    }
}

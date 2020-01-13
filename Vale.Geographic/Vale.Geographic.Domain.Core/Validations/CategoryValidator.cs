using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryValidator(ICategoryRepository categoryRepository)
        {
            ValidateId();
            ValidateName();
            ValidateType();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();

            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateType();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();

            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateName();
                ValidateType();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
            });

            this.categoryRepository = categoryRepository;
        }

        #region Validação do dados

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.CategoryIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.CategoryNameRequired)
                .Length(1, 255).WithMessage(Resources.Validations.CategoryNameLength);
        }

        private void ValidateType()
        {
            RuleFor(o => o.TypeEntitie)
                .NotEmpty().WithMessage(Resources.Validations.CategoryTypeRequired);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.CategoryCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.CategoryLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.CategoryStatusRequired);
        }

        #endregion
    }
}

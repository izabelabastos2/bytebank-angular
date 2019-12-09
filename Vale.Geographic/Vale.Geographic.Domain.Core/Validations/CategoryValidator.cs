using Vale.Geographic.Domain.Entities;
using FluentValidation;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            ValidateId();
            ValidateDescription();
            ValidateType();

            RuleSet("Insert", () =>
            {
                ValidateDescription();
                ValidateType();

            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateDescription();
                ValidateType();

            });

        }

        #region Validação do dados

        private void ValidateId()
        {
            //RuleFor(o => o.Id).NotEmpty().WithMessage(Resources.Validations.CategoryIdRequired);
        }

        private void ValidateDescription()
        {
            //RuleFor(o => o.Description)
            //    .NotEmpty().WithMessage(Resources.Validations.CategoryDescriptionRequired)
            //    .Length(1, 50).WithMessage(Resources.Validations.CategoryDescriptionLength);
        }

        private void ValidateType()
        {
            //RuleFor(o => o.TypeEntitie)
            //    .NotEmpty().WithMessage(Resources.Validations.CategoryTypeRequired);
        }

        #endregion
    }
}

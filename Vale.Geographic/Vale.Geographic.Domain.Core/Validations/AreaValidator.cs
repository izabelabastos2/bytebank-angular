using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using GeoAPI.Geometries;
using FluentValidation;
using System.Linq;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class AreaValidator : AbstractValidator<Area>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IAreaRepository areaRepository;

        public AreaValidator(IAreaRepository areaRepository, ICategoryRepository categoryRepository)
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();
            ValidateLocation();
            ValidateParentId();
            ValidateParent();
            ValidateCategoryId();
            ValidateCategory();
            ValidateAreaDuplicate();


            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateLocation();
                ValidateParentId();
                ValidateParent();
                ValidateCategoryId();
                ValidateCategory();
                ValidateAreaDuplicate();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateLocation();
                ValidateParentId();
                ValidateParent();
                ValidateCategoryId();
                ValidateCategory();
            });

            this.areaRepository = areaRepository;
            this.categoryRepository = categoryRepository;
        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.AreaIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.AreaNameRequired)
                .Length(1, 150).WithMessage(Resources.Validations.AreaNameLength);
        }

        private void ValidateDescription()
        {
            RuleFor(o => o.Description)
               .Length(1, 255).When(x => !string.IsNullOrWhiteSpace(x.Description)).WithMessage(Resources.Validations.AreaDescriptionLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.AreaCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.AreaLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.AreaStatusRequired);
        }

        private void ValidateLocation()
        {
            RuleFor(o => o.Location)
                .NotEmpty().WithMessage(Resources.Validations.AreaLocationRequired)
                .Must(x => !x.OgcGeometryType.Equals(OgcGeometryType.Point)).WithMessage(Resources.Validations.AreaLocationInvalid);
        }

        private void ValidateParentId()
        {
            RuleFor(o => o.ParentId.Value)
               .Must(ExistingArea).When(x => x.ParentId != null).WithMessage(Resources.Validations.AreaNotFound);
        }

        private void ValidateCategoryId()
        {
            RuleFor(o => o.CategoryId)           
                .Must(ExistingCategory).When(x => x.CategoryId != null).WithMessage(Resources.Validations.CategoryNotFound);
        }

        private void ValidateCategory()
        {
            RuleFor(o => o.Category)
                .Must(x => x.TypeEntitie.Equals(TypeEntitieEnum.Area)).When(x => x.Category != null).WithMessage(Resources.Validations.AreaCategoryInvalid); 
        }

        private void ValidateParent()
        {
            RuleFor(o => o.Parent)
               .Must(x => ExistingArea(x.Id)).When(x => x.Parent != null).WithMessage(Resources.Validations.AreaNotFound);
        }

        private void ValidateAreaDuplicate()
        {
            RuleFor(x => x).Must(x => NotExistingArea(x)).WithMessage(Resources.Validations.AreaExisting);
        }

        private bool ExistingArea(Guid areaId)
        {        
            return areaRepository.GetById(areaId) != null ? true : false;
        }

        private bool NotExistingArea(Area area)
        {
            return areaRepository.Get(null, out int total, area.Location).Count() == 0;
        }

        private bool ExistingCategory(Guid? categoryId)
        {
            return categoryId.HasValue && categoryRepository.Get(categoryId.Value, out int total, true) != null ? true : false;
        }

        #endregion Validações de campos
    }
}
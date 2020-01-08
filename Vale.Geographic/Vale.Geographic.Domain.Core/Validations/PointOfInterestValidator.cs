using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using GeoAPI.Geometries;
using FluentValidation;
using System.Linq;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class PointOfInterestValidator : AbstractValidator<PointOfInterest>
    {
        private readonly IPointOfInterestRepository pointOfInterestRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAreaRepository areaRepository;

        public PointOfInterestValidator(IPointOfInterestRepository pointOfInterestRepository, IAreaRepository areaRepository, ICategoryRepository categoryRepository)
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();
            ValidateParentId();
            ValidateCategoryId();
            ValidateCategory();
            ValidateLocation();
            ValidatePointDuplicate();


            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateParentId();
                ValidateCategoryId();
                ValidateCategory();
                ValidateLocation();
                ValidatePointDuplicate();
            });

            RuleSet("Update", () =>
            {
                ValidateId();
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateParentId();
                ValidateCategoryId();
                ValidateCategory();
                ValidateLocation();
            });

            RuleSet("Delete", () =>
            {
                ValidateId();
            });

            this.pointOfInterestRepository = pointOfInterestRepository;
            this.categoryRepository = categoryRepository;
            this.areaRepository = areaRepository;
        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.PointOfInterestIdRequired)
                .Must(ExistingPoint).WithMessage(Resources.Validations.PointOfInterestNotFound);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.PointOfInterestNameRequired)
                .Length(1, 150).WithMessage(Resources.Validations.PointOfInterestNameLength);
        }

        private void ValidateDescription()
        {
            RuleFor(o => o.Description)
                .Length(1, 255).When(x => !string.IsNullOrWhiteSpace(x.Description)).WithMessage(Resources.Validations.PointOfInterestDescriptionLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.PointOfInterestCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.PointOfInterestLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.PointOfInterestStatusRequired);
        }

        private void ValidateLocation()
        {
            RuleFor(o => o.Location)
                .NotEmpty().WithMessage(Resources.Validations.AreaLocationRequired)
                .Must(x => x.OgcGeometryType.Equals(OgcGeometryType.Point)).WithMessage(Resources.Validations.PointOfInterestLocationInvalid);
        }

        private void ValidateParentId()
        {
            RuleFor(o => o.AreaId)
                .NotEmpty().WithMessage(Resources.Validations.PointOfInterestAreaIdRequired)
                .Must(ExistingArea).WithMessage(Resources.Validations.AreaNotFound);
        }

        private void ValidateCategoryId()
        {
            RuleFor(o => o.CategoryId)
                .Must(ExistingCategoria).When(x => x.AreaId != null).WithMessage(Resources.Validations.CategoryNotFound);
        }

        private void ValidateCategory()
        {
            RuleFor(o => o.Category)
                .Must(x => x.TypeEntitie.Equals(TypeEntitieEnum.PointOfInterest)).When(x => x.Category != null).WithMessage(Resources.Validations.PointOfInterestCategoryInvalid);
        }

        private bool ExistingArea(Guid areaId)
        {
            return  areaRepository.GetById(areaId) != null ? true : false;
        }

        private bool ExistingPoint(Guid Id)
        {
            //return pointOfInterestRepository.GetById(Id) != null ? true : false;
            return pointOfInterestRepository.RecoverById(Id) != null ? true : false;
        }

        private void ValidatePointDuplicate()
        {
            RuleFor(x => x).Must(x => NotExistingPoint(x)).WithMessage(Resources.Validations.PointOfInterestExisting);
        }

        private bool NotExistingPoint(PointOfInterest point)
        {
            return pointOfInterestRepository.Get(point.Id, out int total, point.Location).Count() == 0;
        }

        private bool ExistingCategoria(Guid? categoryId)
        {
            return categoryId.HasValue && categoryRepository.Get(categoryId.Value, out int total, true) != null ? true : false;
        }       

        #endregion Validações de campos
    }
}
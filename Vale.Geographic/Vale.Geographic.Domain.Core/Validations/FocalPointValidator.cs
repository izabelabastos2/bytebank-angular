using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class FocalPointValidator : AbstractValidator<FocalPoint>
    {
        private readonly IFocalPointRepository focalPointRepository;
        private readonly IPointOfInterestRepository pointOfInterestRepository;
        private readonly IAreaRepository areaRepository;

        public FocalPointValidator(IFocalPointRepository focalPointRepository, 
                                   IPointOfInterestRepository pointOfInterestRepository,
                                   IAreaRepository areaRepository)
        {
            ValidateId();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateCreatedBy();
            ValidateLastUpdatedBy();
            ValidateStatus();
            ValidateName();
            ValidateMatricula();
            ValidatePointOfInterestId();
            ValidateLocalityId();

            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
                ValidateStatus();
                ValidatePointOfInterestId();
                ValidateLocalityId();
                ValidateMatricula();

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
                ValidatePointOfInterestId();
                ValidateLocalityId();
                ValidateMatricula();

            });

            this.focalPointRepository = focalPointRepository;
            this.pointOfInterestRepository = pointOfInterestRepository;
            this.areaRepository = areaRepository;
        }
        
        #region Validação do dados

        private void ValidateId()
        {
            RuleFor(o => o.Id)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointNameRequired)
                .Length(1, 255).WithMessage(Resources.Validations.FocalPointNameLength);
        }

        private void ValidateMatricula()
        {
            RuleFor(o => o.Matricula)
                 .NotEmpty().WithMessage(Resources.Validations.FocalPointMatriculaRequired)
                .Length(1, 15).WithMessage(Resources.Validations.FocalPointMatriculaLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.FocalPointStatusRequired);
        }

        private void ValidateCreatedBy()
        {
            RuleFor(o => o.CreatedBy)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointCreatedByRequired);
        }

        private void ValidateLastUpdatedBy()
        {
            RuleFor(o => o.LastUpdatedBy)
                .NotEmpty().WithMessage(Resources.Validations.FocalPointLastUpdatedByRequired);
        }

        private void ValidatePointOfInterestId()
        {
            RuleFor(o => o.PointOfInterestId)
                .Must(ExistingPointOfInterest).When(x => x.PointOfInterestId != null)
                .WithMessage(Resources.Validations.PointOfInterestNotFound);
        }

        private bool ExistingPointOfInterest(Guid pointOfInterestId)
        {
            return  pointOfInterestRepository.GetById(pointOfInterestId) != null ? true : false;
        }

        private void ValidateLocalityId()
        {
            RuleFor(o => o.LocalityId)
                .Must(ExistingLocality).When(x => x.LocalityId != null).WithMessage(Resources.Validations.AreaNotFound);
        }

        private bool ExistingLocality(Guid localityId)
        {
            return areaRepository.GetById(localityId) != null ? true : false;
        }

        #endregion
    }
}

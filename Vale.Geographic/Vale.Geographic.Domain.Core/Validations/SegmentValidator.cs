using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities;
using GeoAPI.Geometries;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class SegmentValidator : AbstractValidator<Segment>
    {
        private readonly ISegmentRepository segmentRepository;
        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;

        public SegmentValidator(ISegmentRepository segmentRepository, IRouteRepository routeRepository, IAreaRepository areaRepository)
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();
            ValidateLocation();
            ValidateRouteId();
            ValidateAreaId();
            ValidateCreatedBy();
            ValidateLastUpdatedBy();


            RuleSet("Insert", () =>
            {
                ValidateName();
                ValidateDescription();
                ValidateCreatedAt();
                ValidateLastUpdatedAt();
                ValidateStatus();
                ValidateLocation();
                ValidateRouteId();
                ValidateAreaId();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();

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
                ValidateRouteId();
                ValidateAreaId();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
            });

            this.segmentRepository = segmentRepository;
            this.routeRepository = routeRepository;
            this.areaRepository = areaRepository;
        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id)
              .NotEmpty().WithMessage(Resources.Validations.SegmentIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.SegmentNameRequired)
                .Length(1, 150).WithMessage(Resources.Validations.SegmentNameLength);
        }

        private void ValidateDescription()
        {
            RuleFor(o => o.Description)
                .Length(1, 255).When(x => !string.IsNullOrWhiteSpace(x.Description)).WithMessage(Resources.Validations.SegmentDescriptionLength);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.SegmentCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.SegmentLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.SegmentStatusRequired);
        }
        
        private void ValidateLocation()
        {
            RuleFor(o => o.Location)
                .NotEmpty().WithMessage(Resources.Validations.SegmentLocationRequired);
        }

        private void ValidateAreaId()
        {
            RuleFor(o => o.AreaId)
                .NotEmpty().WithMessage(Resources.Validations.SegmentAreaIdRequired)
                .Must(ExistingArea).WithMessage(Resources.Validations.AreaNotFound);
        }

        private void ValidateRouteId()
        {
            RuleFor(o => o.RouteId)
                .NotEmpty().WithMessage(Resources.Validations.SegmentRouteIdRequired)
                .Must(ExistingRoute).WithMessage(Resources.Validations.RouteNotFound);
        }

        private bool ExistingArea(Guid areaId)
        {
            return areaRepository.GetById(areaId) != null ? true : false;
        }

        private bool ExistingRoute(Guid Id)
        {
            return routeRepository.GetById(Id) != null ? true : false;
        }

        private void ValidateCreatedBy()
        {
            RuleFor(o => o.CreatedBy)
                .NotEmpty().WithMessage(Resources.Validations.SegmentCreatedByRequired);
        }

        private void ValidateLastUpdatedBy()
        {
            RuleFor(o => o.LastUpdatedBy)
                .NotEmpty().WithMessage(Resources.Validations.SegmentLastUpdatedByRequired);
        }
        #endregion Validações de campos
    }
}
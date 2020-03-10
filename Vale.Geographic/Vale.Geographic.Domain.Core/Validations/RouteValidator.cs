using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities;
using GeoAPI.Geometries;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Validations
{
    public class RouteValidator : AbstractValidator<Route>
    {
        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;

        public RouteValidator(IRouteRepository routeRepository, IAreaRepository areaRepository)
        {
            ValidateId();
            ValidateName();
            ValidateDescription();
            ValidateCreatedAt();
            ValidateLastUpdatedAt();
            ValidateStatus();
            ValidateLocation();
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
                ValidateAreaId();
                ValidateCreatedBy();
                ValidateLastUpdatedBy();
            });
            this.routeRepository = routeRepository;
            this.areaRepository = areaRepository;
        }

        #region Validações de campos

        private void ValidateId()
        {
            RuleFor(o => o.Id)
              .NotEmpty().WithMessage(Resources.Validations.RouteIdRequired);
        }

        private void ValidateName()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage(Resources.Validations.RouteNameRequired)
                .Length(1, 150).WithMessage(Resources.Validations.RouteNameLength);
        }

        private void ValidateDescription()
        {
            RuleFor(o => o.Description)
                .Length(1, 255).When(x => !string.IsNullOrWhiteSpace(x.Description)).WithMessage(Resources.Validations.RouteDescriptionLength);
        }

        private void ValidateLocation()
        {
            RuleFor(o => o.Location)
                .NotEmpty().WithMessage(Resources.Validations.RouteLocationRequired);
        }

        private void ValidateCreatedAt()
        {
            RuleFor(o => o.CreatedAt)
                .NotEmpty().WithMessage(Resources.Validations.RouteCreatedAtRequired);
        }

        private void ValidateLastUpdatedAt()
        {
            RuleFor(o => o.LastUpdatedAt)
                .NotEmpty().WithMessage(Resources.Validations.RouteLastUpdatedAtRequired);
        }

        private void ValidateStatus()
        {
            RuleFor(o => o.Status)
                .NotNull().WithMessage(Resources.Validations.RouteStatusRequired);
        }

        private void ValidateAreaId()
        {
            RuleFor(o => o.AreaId)
                .NotEmpty().WithMessage(Resources.Validations.RouteAreaIdRequired)
                .Must(ExistingArea).WithMessage(Resources.Validations.AreaNotFound);
        }

        private bool ExistingArea(Guid areaId)
        {
            return areaRepository.GetById(areaId) != null ? true : false;
        }

        private void ValidateCreatedBy()
        {
            RuleFor(o => o.CreatedBy)
                .NotEmpty().WithMessage(Resources.Validations.RouteCreatedByRequired);
        }

        private void ValidateLastUpdatedBy()
        {
            RuleFor(o => o.LastUpdatedBy)
                .NotEmpty().WithMessage(Resources.Validations.RouteLastUpdatedByRequired);
        }
       
        #endregion Validações de campos
    }
}
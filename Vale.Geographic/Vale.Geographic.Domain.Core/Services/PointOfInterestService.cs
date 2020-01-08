using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using FluentValidation;
using System;

namespace Vale.Geographic.Domain.Core.Services
{
    public class PointOfInterestService : Service<PointOfInterest>, IPointOfInterestService
    {
        private readonly ICategoryRepository categoryRepository;

        public PointOfInterestService(IUnitOfWork context, IPointOfInterestRepository rep, IAreaRepository areaRepository, ICategoryRepository categoryRepository) : base(context, rep)
        {
            Validator = new  PointOfInterestValidator(rep, areaRepository, categoryRepository);
            this.categoryRepository = categoryRepository;
        }

        public override PointOfInterest Insert(PointOfInterest obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.LastUpdatedAt = DateTime.UtcNow;
            obj.Status = true;

            if (obj.CategoryId.HasValue)
                obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public override PointOfInterest Update(PointOfInterest obj)
        {
            obj.LastUpdatedAt = DateTime.UtcNow;

            if (obj.CategoryId.HasValue)
                obj.Category = categoryRepository.GetById(obj.CategoryId.Value);

            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");

            return Repository.Update(obj);
        }
    }
}
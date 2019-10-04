using System;
using FluentValidation;
using Vale.Geographic.Domain.Base;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Domain.Core.Services
{
    public class PointOfInterestService : Service<PointOfInterest>, IPointOfInterestService
    {
        public PointOfInterestService(IUnitOfWork context, IPointOfInterestRepository rep) : base(context, rep)
        {
            Validator = new PointOfInterestValidator();
        }


        public override PointOfInterest Insert(PointOfInterest obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }
    }
}
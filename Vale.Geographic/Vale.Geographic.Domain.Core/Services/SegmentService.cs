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
    public class SegmentService : Service<Segment>, ISegmentService
    {
        public SegmentService(IUnitOfWork context, ISegmentRepository rep) : base(context, rep)
        {
            Validator = new SegmentValidator();
        }


        public override Segment Insert(Segment obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }
    }
}
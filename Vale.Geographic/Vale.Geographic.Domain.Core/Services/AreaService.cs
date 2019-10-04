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
    public class AreaService : Service<Area>, IAreaService
    {
        public AreaService(IUnitOfWork context, IAreaRepository rep) : base(context, rep)
        {
            Validator = new AreaValidator();
        }


        public override Area Insert(Area obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            obj.Status = true;

            return Repository.Insert(obj);
        }
    }
}
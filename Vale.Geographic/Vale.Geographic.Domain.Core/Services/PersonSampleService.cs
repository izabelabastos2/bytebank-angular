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
    public class PersonSampleService : Service<PersonSample>, IPersonSampleService
    {
        public PersonSampleService(IUnitOfWork context, IPersonSampleRepository rep) : base(context, rep)
        {
            Validator = new PersonSampleValidator();
        }


        public override PersonSample Insert(PersonSample obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            if ((DateTime.Now.Year - obj.DateBirth.Year) < 18)
                throw new ValidationException("Registration is not allowed to the under 18 years");

            obj.Active = true;

            return Repository.Insert(obj);
        }
    }
}
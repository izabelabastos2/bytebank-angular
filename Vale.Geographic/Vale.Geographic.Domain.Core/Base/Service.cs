﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Vale.Geographic.Domain.Base.Interfaces;

namespace Vale.Geographic.Domain.Core.Base
{
    [ExcludeFromCodeCoverage]
    public class Service<TEntity> : IService<TEntity>
        where TEntity : class
    {
        public Service(IUnitOfWork context, IRepository<TEntity> repository)
        {
            Repository = repository;
            Context = context;
        }

        protected IRepository<TEntity> Repository { get; }

        protected AbstractValidator<TEntity> Validator { get; set; }

        protected IUnitOfWork Context { get; set; }

        public virtual void Delete(TEntity obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Delete");
            Repository.Delete(obj);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual TEntity GetById(long id)
        {
            return Repository.GetById(id);
        }

        public virtual TEntity Insert(TEntity obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Insert");

            return Repository.Insert(obj);
        }

        public virtual TEntity Update(TEntity obj)
        {
            if (Context.ValidateEntity)
                Validator.ValidateAndThrow(obj, "Update");
            return Repository.Update(obj);
        }
    }
}
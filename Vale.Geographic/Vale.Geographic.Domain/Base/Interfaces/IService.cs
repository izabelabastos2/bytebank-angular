using System;
using System.Collections.Generic;

namespace Vale.Geographic.Domain.Base.Interfaces
{
    public interface IService<TEntity> : IDisposable where TEntity : class
    {
        void Delete(TEntity obj);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(long id);

        TEntity Insert(TEntity obj);

        TEntity Update(TEntity obj);
    }
}
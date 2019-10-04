using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Vale.Geographic.Domain.Base;
using Vale.Geographic.Domain.Base.Interfaces;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Vale.Geographic.Infra.Data.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        public IDbConnection Connection { get; set; }
        protected IUnitOfWork Uow { get; set; }

        public Repository(IUnitOfWork uow)
        {
            this.Uow = uow;
            Connection = uow.Connection;
        }

        public virtual void Delete(Guid id)
        {
            var obj = Activator.CreateInstance<TEntity>();
            obj.Id = id;
            Connection.Delete(obj, Uow.Transaction);
        }
        public virtual void Delete(TEntity obj)
        {
            Connection.Delete(obj, Uow.Transaction);
        }
        public virtual async Task<bool> DeleteAsync(TEntity obj)
        {
            return await Connection.DeleteAsync(obj, Uow.Transaction);
        }
        public virtual void DeleteRange(ICollection<TEntity> t)
        {
            this.Uow.Context.Set<TEntity>().RemoveRange(t);
        }


        public virtual TEntity GetById(Guid id)
        {
            return Connection.Get<TEntity>(id, Uow.Transaction);
        }
        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Connection.GetAsync<TEntity>(id, Uow.Transaction);
        }

        public virtual TEntity Insert(TEntity obj)
        {
            obj.Id = Guid.NewGuid();
            Connection.Insert(obj, Uow.Transaction);
            return obj;
        }
        public virtual async Task<TEntity> InsertAsync(TEntity obj)
        {
            obj.Id = Guid.NewGuid();
            await Connection.InsertAsync(obj, Uow.Transaction);
            return obj;
        }

        public virtual TEntity Update(TEntity obj)
        {
            Connection.Update(obj, Uow.Transaction);
            return obj;
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity obj)
        {
            await Connection.UpdateAsync(obj, Uow.Transaction);
            return obj;
        }


        public virtual IEnumerable<TEntity> GetAll()
        {
            return Connection.GetAll<TEntity>(Uow.Transaction);
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> match, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();

            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            return result.Where(match);
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> match, IFilterParameters parameters, string[] includeFilterFields, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().Where(match);
            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            if (!string.IsNullOrWhiteSpace(parameters.filter))
            {
                result = result.ApplyFilter(parameters.filter, includeFilterFields);
            }

            if (!string.IsNullOrWhiteSpace(parameters.sort))
            {
                result = result.OrderBy(parameters.sort);
            }

            return result;
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> match, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();
            if (includeProperties != null)
            {
                result = includeProperties.Aggregate(result, (current, property) => current.Include(property));
            }

            return await result.SingleOrDefaultAsync(match);
        }

        public virtual IQueryable<TEntity> FilterAll(Expression<Func<TEntity, bool>> match, string filter, string[] includeFilterFields, params string[] includeProperties)
        {
            var result = this.Uow.Context.Set<TEntity>().AsQueryable();
            result = includeProperties.Aggregate(result, (current, property) => current.Include(property));

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.ApplyFilter(filter, includeFilterFields);
            }

            return result.Where(match);
        }

        public virtual IQueryable<TEntity> Set => this.Uow.Context.Set<TEntity>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
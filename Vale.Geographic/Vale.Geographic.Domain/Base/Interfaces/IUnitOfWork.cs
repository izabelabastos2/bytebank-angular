using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vale.Geographic.Domain.Base.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        Guid Id { get; }
        IDbTransaction Transaction { get; }

        DbContext Context { get; set; }

        bool ValidateEntity { get; set; }

        void BeginTransaction();

        void Commit();

        void Rollback();


    }
}
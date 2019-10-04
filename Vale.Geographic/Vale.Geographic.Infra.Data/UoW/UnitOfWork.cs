using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Vale.Geographic.Domain.Base.Interfaces;


namespace Vale.Geographic.Infra.Data.UoW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; set; }
        public DbContext Context { get; set; }

        public Guid Id { get; set; }

        public IDbTransaction Transaction { get; set; }
        public IDbContextTransaction ContextTransaction { get; set; }
        public bool ValidateEntity { get; set; }

        public UnitOfWork(IDbConnection connection, DbContext context)
        {
            this.Context = context;
            this.ValidateEntity = true;
            Id = Guid.NewGuid();
            Connection = connection;
        }

        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Transaction = Connection.BeginTransaction();
            }
            else
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
            }
        }


        public void Commit()
        {
            Transaction.Commit();
            Dispose();
        }


        public void Dispose()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection?.Dispose();
                Connection = null;
            }

            if (Transaction != null)
            {
                Transaction?.Dispose();

                Transaction = null;
            }

            if (Context != null)
            {
                Context?.Dispose();

                Context = null;
            }
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Dispose();
        }


    }
}
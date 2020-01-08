﻿using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System;
using System.Collections.Generic;
using Dapper;
using System.Text;
using Vale.Geographic.Domain.Enumerable;
using System.Data;
using System.Linq;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork context) : base(context)
        {

        }

        public IEnumerable<Category> Get(Guid? id, out int total, bool? active = null, TypeEntitieEnum? TypeEntitie = null, IFilterParameters parameters = null)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT  CAT.[Id],
		                                  CAT.[CreatedAt],
		                                  CAT.[LastUpdatedAt],
		                                  CAT.[Status],
		                                  CAT.[TypeEntitie],
		                                  CAT.[Name],
		                                  COUNT(1) OVER () as Total
	                                FROM dbo.Categorys CAT                             
	                                WHERE 0 = 0 ");

            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND CAT.Id = @Id");
                param.Add("Id", id);
            }

            if (TypeEntitie.HasValue)
            {
                sqlQuery.AppendLine(@" 	AND CAT.TypeEntitie = @TypeEntitie");
                param.Add("TypeEntitie", TypeEntitie.Value);
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND CAT.Status = @Status");
                param.Add("Status", active);
            }        

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY CAT.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                     parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<Category> result = this.Connection.Query<Category>(sqlQuery.ToString(),
                param,
                (IDbTransaction) this.Uow.Transaction);

            total = count;

            return result;
        }

        public Category RecoverById(Guid Id)
        {
            var total = 0;
            return this.Get(Id, out total).FirstOrDefault();
        }
    }    
}

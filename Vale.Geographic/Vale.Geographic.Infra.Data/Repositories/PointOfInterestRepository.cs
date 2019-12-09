using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class PointOfInterestRepository : Repository<PointOfInterest>, IPointOfInterestRepository
    {
        public PointOfInterestRepository(IUnitOfWork context) : base(context)
        {
        }


        public IEnumerable<PointOfInterest> Get(bool? active, Guid categoryId, string sort, int page, int per_page, out int total)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT  P.[Id],
		                                  P.[CreatedAt],
		                                  P.[LastUpdatedAt],
		                                  P.[Status],
		                                  P.[Name],
		                                  P.[Description],
		                                  P.[Location],
		                                  P.[CategoryId],		                                 
		                                  C.[Id],
		                                  C.[CreatedAt],
		                                  C.[LastUpdatedAt],
		                                  C.[Status],
		                                  C.[TypeEntitie],
		                                  C.[Description],
		                                  COUNT(1) OVER () as Total
                                   FROM dbo.PointOfInterest P
                                   INNER JOIN dbo.Area A ON P.AreaId = A.Id
                                   LEFT JOIN dbo.Categorys C ON P.CategoryId = C.Id AND C.[TypeEntitie] = 1
                                   WHERE 0 = 0 ");

            if (!categoryId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND P.CategoryId <> NULL 
                                       AND C.Id = @CategoryId");
                param.Add("CategoryId", categoryId);
            }

            var count = 0;

            var result = this.Connection.Query<PointOfInterest, Category, int, PointOfInterest>(sqlQuery.ToString(),
           (p, c, t) => { p.Category = c; count = t; return p; },
           param: param,
           transaction: this.Uow.Transaction,
           splitOn: "Id, Id, Total");

            total = count;

            return result;
        }
    }
}
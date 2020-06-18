using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using System;
using System.Data;
using System.Text;
using Dapper;
using NetTopologySuite.IO;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class FocalPointRepository : Repository<FocalPoint>, IFocalPointRepository
    {
        public FocalPointRepository(IUnitOfWork context) : base(context)
        {
        }

        public IEnumerable<FocalPoint> Get(out int total, bool? active, string matricula, Guid? localityId, Guid? pointOfInterestId, IFilterParameters parameters)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT FP.[Id],
		                                 FP.[CreatedAt],
		                                 FP.[LastUpdatedAt],
		                                 FP.[CreatedBy],
		                                 FP.[LastUpdatedBy],
		                                 FP.[Status],
		                                 FP.[Name],
		                                 FP.[Matricula],
		                                 FP.[PhoneNumber],
		                                 FP.[LocalityId],
		                                 FP.[PointOfInterestId],
		                                 POINT.[Id],
		                                 POINT.[CreatedAt],
		                                 POINT.[LastUpdatedAt],
		                                 POINT.[Status],
		                                 POINT.[Name],
		                                 POINT.[Description],
		                                 POINT.[CategoryId],
		                                 POINT.[AreaId],
		                                 POINT.[Icon],
                                         POINT.[Location].ToString() as Location,
		                                 AREA.[Id],
		                                 AREA.[CreatedAt],
		                                 AREA.[LastUpdatedAt],
		                                 AREA.[Status],
		                                 AREA.[Name],
		                                 AREA.[Description],
		                                 AREA.[CategoryId],	
		                                 AREA.[ParentId],
		                                 AREA.[Color],
		                                 COUNT(1) OVER () as Total
                                    FROM [dbo].[FocalPoints] FP
                                    INNER JOIN [dbo].[PointOfInterest] POINT on  POINT.Id = FP.[PointOfInterestId]
                                    INNER JOIN [dbo].[Area] AREA ON AREA.Id = FP.[LocalityId]
                                    WHERE 0 = 0");


            if (localityId.HasValue && !localityId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND FP.LocalityId = @LocalityId");
                param.Add("LocalityId", localityId);
            }

            if (pointOfInterestId.HasValue && !pointOfInterestId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" FP.PointOfInterestId = @PointOfInterestId");
                param.Add("PointOfInterestId", pointOfInterestId);
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND FP.Status = @Status");
                param.Add("Status", active);
            }

            if (!string.IsNullOrWhiteSpace(matricula))
            {
                sqlQuery.AppendLine(@" AND FP.Matricula = @Matricula");
                param.Add("Matricula", matricula);
            }



            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY FP.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                     parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<FocalPoint> result = this.Connection.Query<FocalPoint, PointOfInterest, string,  Area, int, FocalPoint>(sqlQuery.ToString(),
                (c, p, geo, l, t) => {
                    count = t;
                    p.Location = new WKTReader().Read(geo);
                    c.PointOfInterest = p;
                    c.Locality = l;

                    return c;
                },
                param,
                (IDbTransaction)this.Uow.Transaction,
                splitOn: "Id, Id, Location, Id, Total");

            total = count;

            return result;
        }
    }
}

using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class AuditoryRepository : Repository<Auditory>, IAuditoryRepository
    {
        public AuditoryRepository(IUnitOfWork context) : base(context)
        {
        }

        public IEnumerable<Auditory> Get(Guid? areaId, Guid? pointOfInterestId, Guid? categoryId, TypeEntitieEnum? typeEntitie, out int total, IFilterParameters parameters )
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT AUD.[Id]
                                        ,AUD.[CreatedAt]
                                        ,AUD.[LastUpdatedAt]
                                        ,AUD.[CreatedBy]
                                        ,AUD.[Status]
                                        ,AUD.[TypeEntitie]
                                        ,AUD.[OldValue]
                                        ,AUD.[NewValue]
                                        ,AUD.[AreaId]
                                        ,AUD.[PointOfInterestId]
                                        ,AUD.[CategoryId]
                                        ,COUNT(1) OVER () as Total
                                    FROM [dbo].[Auditory] AUD
                                    WHERE 0 = 0 ");


            if (areaId.HasValue && !areaId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AUD.AreaId = @AreaId");
                param.Add("AreaId", areaId);
            }

            if (pointOfInterestId.HasValue && !pointOfInterestId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AUD.PointOfInterestId = @PointOfInterestId");
                param.Add("PointOfInterestId", pointOfInterestId);
            }

            if (categoryId.HasValue && !categoryId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AUD.CategoryId = @CategoryId");
                param.Add("CategoryId", categoryId);
            }

            if (typeEntitie.HasValue)
            {
                sqlQuery.AppendLine(@" 	AND AUD.TypeEntitie = @TypeEntitie");
                param.Add("TypeEntitie", typeEntitie.Value);
            }           

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY AUD.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                     parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<Auditory> result = this.Connection.Query<Auditory>(sqlQuery.ToString(),
               param,
               (IDbTransaction)this.Uow.Transaction);

            total = count;
            return result;
        }
    }
}

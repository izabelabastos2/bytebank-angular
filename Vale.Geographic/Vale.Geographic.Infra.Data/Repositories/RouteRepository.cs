using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using NetTopologySuite.IO;
using System.Text;
using System.Linq;
using Dapper;
using System;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(IUnitOfWork context) : base(context)
        {
        }

        public IEnumerable<Route> Get(Guid? id, out int total, bool? active = null, Guid? areaId = null, IFilterParameters parameters = null)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@" SELECT RT.[Id],
                                          RT.[CreatedAt],
                                          RT.[LastUpdatedAt],
                                          RT.[Status],
                                          RT.[Name],
                                          RT.[Description],
                                          RT.[Length],
                                          RT.[Location].ToString() as Location,
                                          RT.[AreaId],
	                                      AREA.[Id],
	                                      AREA.[CreatedAt],
	                                      AREA.[LastUpdatedAt],
   	                                      AREA.[Status],
	                                      AREA.[Name],
	                                      AREA.[Description],
	                                      AREA.[CategoryId],	
	                                      AREA.[ParentId],
	                                      AREA.[Location].ToString() as LocationArea,
	                                      COUNT(1) OVER () as Total
                                      FROM [dbo].[Route] RT
                                     INNER JOIN  [dbo].[Area] AREA ON AREA.Id = RT.AreaId
                                     WHERE 0 = 0");

            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND RT.Id = @Id");
                param.Add("Id", id);
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND RT.Status = @Status");
                param.Add("Status", active);
            }

            if (areaId.HasValue && !areaId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND RT.AreaId = @AreaId");
                param.Add("AreaId", areaId);
            }

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY RT.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                    parameters.page, parameters.per_page));
            }


            var count = 0;

            IEnumerable<Route> result = this.Connection.Query<Route, string, Area, string, int, Route>(sqlQuery.ToString(),
             (r, geo, a, geoa, t) => {

                 r.Location = new WKTReader().Read(geo);
                 a.Location = new WKTReader().Read(geoa);                
                 r.Area = a;
                 count = t;

                 return r;
             },
             param: param,
             splitOn: "Id, Location, Id, LocationArea, Total");

            total = count;
            return result;
        }

        public Route RecoverById(Guid Id)
        {
            var total = 0;
            return this.Get(Id, out total).FirstOrDefault();
        }
    }
}
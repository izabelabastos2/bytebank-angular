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
    public class SegmentRepository : Repository<Segment>, ISegmentRepository
    {
        public SegmentRepository(IUnitOfWork context) : base(context)
        {
        }
        
        public IEnumerable<Segment> Get(Guid? id, out int total, bool? active = null, Guid? areaId = null, Guid? routeId = null, IFilterParameters parameters = null)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@" SELECT  SEG.[Id],
	                                       SEG.[CreatedAt],
	                                       SEG.[LastUpdatedAt],
	                                       SEG.[Status],
	                                       SEG.[Name],
	                                       SEG.[Description],
	                                       SEG.[Length],
	                                       SEG.[RouteId],
	                                       SEG.[AreaId],
	                                       SEG.[Location].ToString() as Location,
	                                       AREA.[Id],
	                                       AREA.[CreatedAt],
	                                       AREA.[LastUpdatedAt],
   	                                       AREA.[Status],
	                                       AREA.[Name],
	                                       AREA.[Description],
	                                       AREA.[CategoryId],	
	                                       AREA.[ParentId],
	                                       AREA.[Location].ToString() as LocationArea,
	                                       RT.[Id],
	                                       RT.[CreatedAt],
	                                       RT.[LastUpdatedAt],
	                                       RT.[Status],
	                                       RT.[Name],
	                                       RT.[Description],
	                                       RT.[Length],
	                                       RT.[AreaId],
	                                       RT.[Location].ToString() as LocationRoute,
	                                       COUNT(1) OVER () as Total
                                      FROM [dbo].[Segment] SEG
                                    INNER JOIN  dbo.Area AREA ON AREA.Id = SEG.AreaId
                                    INNER JOIN [dbo].[Route] RT ON RT.Id = SEG.RouteId
                                    WHERE 0 = 0");


            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND SEG.Id = @Id");
                param.Add("Id", id);
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND SEG.Status = @Status");
                param.Add("Status", active);
            }

            if (areaId.HasValue && !areaId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND SEG.AreaId = @AreaId");
                param.Add("AreaId", areaId);
            }

            if (routeId.HasValue && !routeId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND SEG.RouteId = @RouteId");
                param.Add("RouteId", routeId);
            }
            
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY SEG.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                    parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<Segment> result = this.Connection.Query<Segment, string, Area, string, Route, string, int, Segment>(sqlQuery.ToString(),
             (s, geo, a, geoa, r, geor, t) => {

                 s.Location = new WKTReader().Read(geo);
                 a.Location = new WKTReader().Read(geoa);
                 r.Location = new WKTReader().Read(geor);
                 s.Area = a;
                 s.Route = r;               
                 count = t;

                 return s;
             },
             param: param,
             splitOn: "Id, Location, Id, LocationArea, Id, LocationRoute, Total");

            total = count;
            return result;
        }

        public Segment RecoverById(Guid Id)
        {
            var total = 0;
            return this.Get(Id, out total).FirstOrDefault();
        }
    }
}
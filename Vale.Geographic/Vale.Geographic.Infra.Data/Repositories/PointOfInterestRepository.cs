using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using System.Text;
using System.Linq;
using System;
using Dapper;
using System.Data;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class PointOfInterestRepository : Repository<PointOfInterest>, IPointOfInterestRepository
    {
        private readonly IConfiguration configuration;
        private static string AreaDistance { get; set; }

        public PointOfInterestRepository(IUnitOfWork context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;

            AreaDistance = this.configuration.GetSection("Distance:AreaDistance").Value;
        }

        public IEnumerable<PointOfInterest> Get(Guid? Id, out int total, IGeometry location = null, IGeometry point = null, bool? active = null, Guid? categoryId = null, Guid? areaId = null, IFilterParameters parameters = null)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT  POINT.[Id],
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
		                                  AREA.[Location].ToString() as LocationArea,
		                                  CAT.[Id],
		                                  CAT.[CreatedAt],
		                                  CAT.[LastUpdatedAt],
		                                  CAT.[Status],
		                                  CAT.[TypeEntitie],
		                                  CAT.[Name],
		                                  COUNT(1) OVER () as Total
                                  FROM dbo.PointOfInterest POINT
                                  LEFT JOIN dbo.Area AREA ON POINT.AreaId = AREA.Id
                                  LEFT JOIN dbo.Categorys CAT ON POINT.CategoryId = CAT.Id AND CAT.[TypeEntitie] = 1
                                  WHERE 0 = 0 ");

            if (Id.HasValue && !Id.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND POINT.Id = @Id");
                param.Add("Id", Id);
            }

            if (categoryId.HasValue && !categoryId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND POINT.CategoryId IS NOT NULL 
                                       AND CAT.Id = @CategoryId");
                param.Add("CategoryId", categoryId);
            }

            if (point != null)
            {              
                sqlQuery.AppendLine(string.Format(@"  AND convert(decimal(18,2), 
                                        POINT.[Location].MakeValid().STDistance('{0}') / 1000) < {1}",
                                        point.ToString(),
                                        AreaDistance));
            }

            if (location != null)
            {
                sqlQuery.AppendLine(@"  AND POINT.[Location].STEquals(geography::STGeomFromText(@Point, 4326).MakeValid()) = 1");
                param.Add("@Point", location.ToString());
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND POINT.Status = @Status");
                param.Add("Status", active);
            }

            if (areaId.HasValue && !areaId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND POINT.AreaId = @AreaId");
                param.Add("AreaId", areaId);
            }

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY POINT.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                     parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<PointOfInterest> result = this.Connection.Query<PointOfInterest, string, Area, string, Category, int, PointOfInterest>(sqlQuery.ToString(),
                (p, geo, a, geoa, c, t) => {
            
                    p.Category = c;
                    p.Location = new WKTReader().Read(geo);

                    if (p.AreaId.HasValue)
                    {
                        p.Area = a;
                        a.Location = new WKTReader().Read(geoa);
                    }

                    count = t;
                    return p;
                },
                param: param,
                transaction: (IDbTransaction)this.Uow.Transaction,
                splitOn: "Id, Location, Id, LocationArea, Id, Total");

            total = count;

            return result;
        }

        public override PointOfInterest Insert(PointOfInterest point)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendLine(@" INSERT INTO [dbo].[PointOfInterest]
                                               ([Id]
                                               ,[CreatedAt]
                                               ,[LastUpdatedAt]
                                               ,[Status]
                                               ,[Name]
                                               ,[Description]
                                               ,[Location]
                                               ,[AreaId]
                                               ,[CategoryId]
                                               ,[Icon])
                                         VALUES
                                              (@Id
		                                      ,@CreatedAt
		                                      ,@LastUpdatedAt
		                                      ,@Status 
		                                      ,@Name 
		                                      ,@Description
		                                      ,geography::STGeomFromText(@Location, 4326).MakeValid()
		                                      ,@AreaId
		                                      ,@CategoryId
                                              ,@Icon) ");

            point.Id = Guid.NewGuid();

            var param = new DynamicParameters();
            param.Add("Id", point.Id);
            param.Add("CreatedAt", point.CreatedAt);
            param.Add("LastUpdatedAt", point.LastUpdatedAt);
            param.Add("Status", point.Status);
            param.Add("Name", point.Name);
            param.Add("Description", point.Description);
            param.Add("AreaId", point.AreaId);
            param.Add("CategoryId", point.CategoryId);
            param.Add("Location", point.Location.ToString());
            param.Add("Icon", point.Icon);


            this.Uow.Connection.Execute(sqlQuery.ToString(),
                param,
                (IDbTransaction)this.Uow.Transaction);

            return point;
        }

    }
}
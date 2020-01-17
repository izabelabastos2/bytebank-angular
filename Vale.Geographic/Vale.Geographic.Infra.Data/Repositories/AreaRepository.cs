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
using System.Data;
using System;
using Dapper;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        private readonly IConfiguration configuration;

        private static string AreaDistance { get; set; }

        public AreaRepository(IUnitOfWork context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;

            AreaDistance = this.configuration.GetSection("Distance:AreaDistance").Value;
        }

        public IEnumerable<Area> Get( Guid? id, out int total, IGeometry location = null, IGeometry point = null, bool? active = null, Guid? categoryId = null, Guid? parentId = null, IFilterParameters parameters = null)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT  AREA.[Id],
		                                  AREA.[CreatedAt],
		                                  AREA.[LastUpdatedAt],
		                                  AREA.[Status],
		                                  AREA.[Name],
		                                  AREA.[Description],
		                                  AREA.[CategoryId],	
		                                  AREA.[ParentId],
	                                      AREA.[Color],
		                                  AREA.[Location].ToString() as Location,		
		                                  CAT.[Id],
		                                  CAT.[CreatedAt],
		                                  CAT.[LastUpdatedAt],
		                                  CAT.[Status],
		                                  CAT.[TypeEntitie],
		                                  CAT.[Name],
		                                  APRT.[Id],
		                                  APRT.[CreatedAt],
		                                  APRT.[LastUpdatedAt],
		                                  APRT.[Status],
		                                  APRT.[Name],
		                                  APRT.[Description],
		                                  APRT.[CategoryId],	
		                                  APRT.[ParentId],
	                                      APRT.[Color],
		                                  APRT.[Location].ToString() as LocationParent,		
		                                  COUNT(1) OVER () as Total
	                                FROM [dbo].[Area] AREA
	                                LEFT JOIN [dbo].[Area] APRT ON APRT.Id = AREA.ParentId
	                                LEFT JOIN [dbo].[Categorys] CAT ON AREA.CategoryId = CAT.Id AND CAT.[TypeEntitie] = 2
	                                WHERE 0 = 0 ");

            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AREA.Id = @Id");
                param.Add("Id", id);
            }

            if (categoryId.HasValue && !categoryId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AREA.CategoryId IS NOT NULL 
                                       AND CAT.Id = @CategoryId");
                param.Add("CategoryId", categoryId);
            }

            if (point != null)
            {              
                sqlQuery.AppendLine(string.Format(@"  AND convert(decimal(18,2), 
                                        AREA.[Location].MakeValid().STDistance('{0}') / 1000) < {1}",
                                        point.ToString(),
                                        AreaDistance));
            }

            if (location != null)
            {
                sqlQuery.AppendLine(@"  AND AREA.[Location].MakeValid().STEquals(geography::STGeomFromText(@Device, 4326).MakeValid()) = 1");
                param.Add("@Device", location.ToString());
            }

            if (active.HasValue)
            {
                sqlQuery.AppendLine(@" AND AREA.Status = @Status");
                param.Add("Status", active);
            }

            if (parentId.HasValue && !parentId.Equals(Guid.Empty))
            {
                sqlQuery.AppendLine(@" AND AREA.ParentId = @ParentId");
                param.Add("ParentId", parentId);
            }

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.sort))
            {
                sqlQuery.AppendLine(string.Format(@" ORDER BY AREA.{0}", parameters.sort));
            }

            if (parameters != null && parameters.page > 0 && parameters.per_page > 0)
            {
                sqlQuery.AppendLine(string.Format(@"
                    OFFSET ({0}-1)*{1} ROWS FETCH NEXT {1} ROWS ONLY",
                    parameters.page, parameters.per_page));
            }

            var count = 0;

            IEnumerable<Area> result = this.Connection.Query<Area, string, Category, Area, string, int, Area>(sqlQuery.ToString(),
             (a, g, c, p, geo, t) => {
                 a.Category = c; 
                 a.Location = new WKTReader().Read(g);

                 if (a.ParentId.HasValue)
                 {
                     p.Location = new WKTReader().Read(geo);
                     a.Parent = p;
                 }
                 count = t;
                 return a;
             },
             param: param,
             transaction: this.Uow.Transaction,
             splitOn: "Id, Location, Id, Id, LocationParent, Total");          

            total = count;
            return result;
        }

        public override Area Insert(Area area)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendLine(@" INSERT INTO [dbo].[Area]
                                       ([Id]
                                       ,[CreatedAt]
                                       ,[LastUpdatedAt]
                                       ,[Status]
                                       ,[Name]
                                       ,[Description]
		                               ,[ParentId]
                                       ,[CategoryId]
                                       ,[Location]
                                       ,[Color])
                                 VALUES
                                       (@Id
                                       ,@CreatedAt
                                       ,@LastUpdatedAt
                                       ,@Status 
                                       ,@Name 
                                       ,@Description
                                       ,@ParentId
                                       ,@CategoryId
		                               ,geography::STGeomFromText(@Location, 4326).MakeValid()
                                       ,@Color) ");

            area.Id = Guid.NewGuid();

            var param = new DynamicParameters();
            param.Add("Id", area.Id);
            param.Add("CreatedAt", area.CreatedAt);
            param.Add("LastUpdatedAt", area.LastUpdatedAt);
            param.Add("Status", area.Status);
            param.Add("Name", area.Name);
            param.Add("Color", area.Color);
            param.Add("Description", area.Description);
            param.Add("ParentId", area.ParentId);
            param.Add("CategoryId", area.CategoryId);
            param.Add("Location", area.Location.ToString());

            this.Uow.Connection.Execute(sqlQuery.ToString(),
                param,
                (IDbTransaction)this.Uow.Transaction
                );
           return area;
        }

    }
}
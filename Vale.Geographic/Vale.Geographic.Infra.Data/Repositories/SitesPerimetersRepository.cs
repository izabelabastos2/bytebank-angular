using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using System.Text;
using System.Data;
using System;
using Dapper;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class SitesPerimetersRepository : Repository<SitesPerimeter>, ISitesPerimetersRepository
    {
        public SitesPerimetersRepository(IUnitOfWork context) : base(context)
        {
        }

        public override SitesPerimeter Insert(SitesPerimeter area)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendLine(@" INSERT INTO  [dbo].[SitesPerimeters]
                                   ([Id]
                                   ,[SiteId]
                                   ,[AreaId]
                                   ,[CreatedAt]
                                   ,[LastUpdatedAt]
                                   ,[Status]
                                   ,[CreatedBy]
                                   ,[LastUpdatedBy])
                                 VALUES
                                       (@Id
                                       ,@SiteId 
                                       ,@AreaId
                                       ,@CreatedAt
                                       ,@LastUpdatedAt
                                       ,@Status 
                                       ,@CreatedBy
                                       ,@LastUpdatedBy) ");
            area.Id = Guid.NewGuid();

            var param = new DynamicParameters();
            param.Add("Id", area.Id);
            param.Add("CreatedAt", area.CreatedAt);
            param.Add("LastUpdatedAt", area.LastUpdatedAt);
            param.Add("Status", area.Status);
            param.Add("AreaId", area.AreaId);
            param.Add("SiteId", area.SiteId);
            param.Add("CreatedBy", area.CreatedBy);
            param.Add("LastUpdatedBy", area.LastUpdatedBy);

            this.Uow.Connection.Execute(sqlQuery.ToString(),
                param,
                (IDbTransaction)this.Uow.Transaction
                );
            return area;
        }
    }
}
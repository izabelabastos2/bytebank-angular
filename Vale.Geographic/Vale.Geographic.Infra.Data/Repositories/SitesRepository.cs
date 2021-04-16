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
    public class SitesRepository : Repository<Site>, ISitesRepository
    {

        public SitesRepository(IUnitOfWork context) : base(context)
        {
        }

        public string GetSiteIdByCode(string code)
        {
            if (String.IsNullOrEmpty(code)){

                return null;
            } else {
                
                if (code.Length == 1)
                    code =  "000"+code;
                else if (code.Length == 2)
                    code =  "00"+code;
                else if (code.Length == 3)
                    code =  "0"+code;                
            }

            var sql = $@"SELECT Id FROM Sites WHERE Code = @Code";

            var result = Connection.QuerySingleOrDefault<Guid>(sql, new { Code = code }, this.Uow.Transaction);

            if (result != null)
                return result.ToString();

            return null;
        }

       
    }
}
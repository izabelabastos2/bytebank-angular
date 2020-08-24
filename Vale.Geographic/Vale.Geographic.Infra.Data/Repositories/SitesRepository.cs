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

       
    }
}
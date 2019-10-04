using System.Collections.Generic;
using Dapper;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class SegmentRepository : Repository<Segment>, ISegmentRepository
    {
        public SegmentRepository(IUnitOfWork context) : base(context)
        {
        }


        public IEnumerable<Segment> Get(bool? active, string sort, int page, int per_page, out int total)
        {
            var sql = @"SELECT [Id],
                    P.[Active],
                    P.[FirstName],
                    P.[LastName],
                    P.[DateBirth],
                    P.[Type],
                    COUNT(1) OVER () as Total
                FROM PersonSamples P
                WHERE (@Active IS NULL OR  P.Active = @Active) ";

            sql += string.Format(@"
                ORDER BY P.{0}
                OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY", sort, page, per_page);

            var count = 0;

            var result = Connection.Query<Segment, int, Segment>(sql,
                (p, t) =>
                {
                    count = t;
                    return p;
                },
                splitOn: "Total",
                param: new { Active = active });
            total = count;
            return result;
        }
    }
}
﻿using System.Collections.Generic;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface ISegmentRepository : IRepository<Segment>
    {
        IEnumerable<Segment> Get(bool? active, string sort, int page, int per_page, out int total);
    }
}
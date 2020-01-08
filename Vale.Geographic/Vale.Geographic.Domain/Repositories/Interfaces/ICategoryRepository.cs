using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> Get(Guid? id, out int total, bool? active = null, TypeEntitieEnum? TypeEntitie = null, IFilterParameters parameters = null);

        Category RecoverById(Guid Id);
    }
}

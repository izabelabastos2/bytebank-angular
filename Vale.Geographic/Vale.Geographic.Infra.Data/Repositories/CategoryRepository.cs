using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork context) : base(context)
        {

        }
    }    
}

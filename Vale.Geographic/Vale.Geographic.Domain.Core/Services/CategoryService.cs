using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Base;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Domain.Core.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork context, ICategoryRepository rep) : base(context, rep)
        {
            Validator = new CategoryValidator();
        }
    }
}

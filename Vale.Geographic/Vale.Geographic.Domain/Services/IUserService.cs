using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Base.Interfaces;

namespace Vale.Geographic.Domain.Services
{
    public interface IUserService : IService<User>
    {
        void InsertAuditory(User newObj, User oldObj);
    }
}

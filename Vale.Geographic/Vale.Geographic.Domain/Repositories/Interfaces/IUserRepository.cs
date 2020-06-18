using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Base.Interfaces;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByMatricula(string matricula);

    }
}

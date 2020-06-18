using Vale.Geographic.Domain.Entities.Authorization;
using System.Threading.Tasks;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface IJWTRepository
    {
        Task<Token> Auth();

    }
}

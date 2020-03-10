using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface IRouteService : IService<Route>
    {
        void InsertAuditory(Route newObj, Route oldObj);
    }
}
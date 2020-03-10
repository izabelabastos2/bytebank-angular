using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface IAreaService : IService<Area>
    {
        void InsertAuditory(Area newObj, Area oldObj);
    }
}
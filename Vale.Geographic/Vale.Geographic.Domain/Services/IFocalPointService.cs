using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface IFocalPointService : IService<FocalPoint>
    {
        void InsertAuditory(FocalPoint newObj, FocalPoint oldObj);
    }
}

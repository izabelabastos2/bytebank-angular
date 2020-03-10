using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface IPointOfInterestService : IService<PointOfInterest>
    {
        void InsertAuditory(PointOfInterest newObj, PointOfInterest oldObj);
    }
}
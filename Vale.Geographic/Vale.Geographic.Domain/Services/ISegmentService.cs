using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface ISegmentService : IService<Segment>
    {
        void InsertAuditory(Segment newObj, Segment oldObj);
    }
}
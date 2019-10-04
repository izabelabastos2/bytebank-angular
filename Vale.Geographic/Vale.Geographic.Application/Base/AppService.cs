using AutoMapper;
using Vale.Geographic.Domain.Base.Interfaces;

namespace Vale.Geographic.Application.Base
{
    public class AppService
    {
        public AppService(IUnitOfWork uoW, IMapper Mapper)
        {
            UoW = uoW;
            this.Mapper = Mapper;
        }

        protected IUnitOfWork UoW { get; set; }
        protected IMapper Mapper { get; set; }
    }
}
using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;
using Vale.Geographic.Application.AutoMapper;

namespace Vale.Geographic.Infra.CrossCutting.IoC
{
    public class MapperProvider
    {
        private readonly Container _container;

        public MapperProvider(Container container)
        {
            _container = container;
        }

        public IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(_container.GetInstance);

            var mc = AutoMapperConfig.RegisterMappings();
            mc.AssertConfigurationIsValid();

            IMapper m = new Mapper(mc, t => _container.GetInstance(t));

            return m;
        }
    }
}
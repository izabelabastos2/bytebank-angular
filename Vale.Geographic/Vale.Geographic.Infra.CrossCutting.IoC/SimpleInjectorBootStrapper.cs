using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using Vale.Geographic.Application.Core.Services;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Context;
using Vale.Geographic.Infra.Data.Repositories;
using Vale.Geographic.Infra.Data.UoW;

namespace Vale.Geographic.Infra.CrossCutting.IoC
{
    public class SimpleInjectorBootStrapper
    {
        private Container container = new Container();

        private string ConnectionString { get; set; }

        public void InitializeContainer(IApplicationBuilder app, IConfiguration configuration)
        {
            container.Register(() => configuration, Lifestyle.Scoped);

            ConnectionString = configuration.GetConnectionString("DefaultConnection");


            // Add application presentation components:
            if (app != null)
            {
                container.RegisterMvcControllers(app);
                container.RegisterMvcViewComponents(app);

                // Cross-wire ASP.NET services (if any). For instance:
                container.CrossWire<ILoggerFactory>(app);

                container.RegisterConditional(
                    typeof(ILogger),
                    c => typeof(Logger<>).MakeGenericType(c.Consumer.ImplementationType),
                    Lifestyle.Singleton,
                    c => true);
            }

            // Add application services. For instance:
            container.Register<IDbConnection>(() => new SqlConnection(ConnectionString), Lifestyle.Scoped);

            container.Register<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions() { SizeLimit = 5000 }), Lifestyle.Singleton);

            container.Register<DbContextOptions>(() =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlServer(ConnectionString, x => x.UseNetTopologySuite());

                return optionsBuilder.Options;
            }, Lifestyle.Scoped);

            container.Register<DbContext, DatabaseContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            container.Register<ISitesRepository, SitesRepository>(Lifestyle.Scoped);

            container.Register<IPerimeterAppService, PerimeterAppService>(Lifestyle.Scoped);
            container.Register<ISitesPerimetersRepository, SitesPerimetersRepository>(Lifestyle.Scoped);

            container.Register<IAreaAppService, AreaAppService>(Lifestyle.Scoped);
            container.Register<IAreaService, AreaService>(Lifestyle.Scoped);
            container.Register<IAreaRepository, AreaRepository>(Lifestyle.Scoped);

            container.Register<IPointOfInterestAppService, PointOfInterestAppService>(Lifestyle.Scoped);
            container.Register<IPointOfInterestService, PointOfInterestService>(Lifestyle.Scoped);
            container.Register<IPointOfInterestRepository, PointOfInterestRepository>(Lifestyle.Scoped);

            container.Register<IRouteAppService, RouteAppService>(Lifestyle.Scoped);
            container.Register<IRouteService, RouteService>(Lifestyle.Scoped);
            container.Register<IRouteRepository, RouteRepository>(Lifestyle.Scoped);

            container.Register<ISegmentAppService, SegmentAppService>(Lifestyle.Scoped);
            container.Register<ISegmentService, SegmentService>(Lifestyle.Scoped);
            container.Register<ISegmentRepository, SegmentRepository>(Lifestyle.Scoped);

            container.Register<ICategoryAppService, CategoryAppService>(Lifestyle.Scoped);
            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<ICategoryRepository, CategoryRepository>(Lifestyle.Scoped);

            container.Register<IAuditoryAppService, AuditoryAppService>(Lifestyle.Scoped);
            container.Register<IAuditoryService, AuditoryService>(Lifestyle.Scoped);
            container.Register<IAuditoryRepository, AuditoryRepository>(Lifestyle.Scoped);

            container.Register<IFocalPointAppService, FocalPointAppService>(Lifestyle.Scoped);
            container.Register<IFocalPointService, FocalPointService>(Lifestyle.Scoped);
            container.Register<IFocalPointRepository, FocalPointRepository>(Lifestyle.Scoped);

            container.Register<IUserAppService, UserAppService>(Lifestyle.Scoped);
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);

            container.Register<INotificationAppService, NotificationAppService>(Lifestyle.Scoped);
            container.Register<INotificationRepository, NotificationRepository>(Lifestyle.Scoped);
            container.Register<INotificationAnswerService, NotificationAnswerService>(Lifestyle.Scoped);
            container.Register<INotificationAnswerRepository, NotificationAnswerRepository>(Lifestyle.Scoped);

            container.Register<IJWTRepository, JWTRepository>(Lifestyle.Scoped);


            container.RegisterSingleton(() => GetMapper(container));
        }

        public void ConfigureEntityFramework(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(
                options => { options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()); });
        }

        public void SetDefaultScopedLifestyle(ScopedLifestyle scope = null)
        {
            container.Options.DefaultScopedLifestyle = scope ?? new AsyncScopedLifestyle();
        }

        public void IntegrateSimpleInjector(IServiceCollection services)
        {
            SetDefaultScopedLifestyle();
            services.AddAutoMapper();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));


            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);

            services.AddScoped<DatabaseContext>();
        }

        public void SetContainer(Container container)
        {
            this.container = container;
        }

        public void Verify()
        {
            container.Verify();
        }

        private IMapper GetMapper(Container container)
        {
            var mp = container.GetInstance<MapperProvider>();
            return mp.GetMapper();
        }
    }
}
using Bogus;
using FluentAssertions;
using GeoAPI.Geometries;
using GeoJSON.Net;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Xunit;

namespace Vale.Geographic.Test.Services
{
    public class RouteServiceTest
    {
        private readonly Faker faker;
        private readonly Area area;
        private readonly Route route;
        private readonly Category category;

        private readonly RouteService routeService;
        private readonly RouteValidator routeValidator;

        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IUnitOfWork unitOfWork;

        public RouteServiceTest()
        {
            faker = new Faker();
            this.category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.Area)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.area = new Faker<Area>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.CategoryId, category.Id)
                .RuleFor(u => u.Category, category)
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            this.route = new Faker<Route>()
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Length, (f, u) => f.Random.Double())
                .RuleFor(u => u.Location, MontarGeometry(CreateLineString()))
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.routeRepository = Substitute.For<IRouteRepository>();
            this.areaRepository = Substitute.For<IAreaRepository>();
            this.unitOfWork = Substitute.For<IUnitOfWork>();

            this.routeService = new RouteService(unitOfWork, routeRepository, areaRepository);
        }

        #region Insert

        [Fact]
        public void ValidateInsert_Success()
        {
            routeRepository.Insert(route).Returns(x =>
            {
                route.Id = Guid.NewGuid();
                return route;
            });

            var routeReturn = routeService.Invoking(y => y.Insert(route))
                .Should().NotThrow().Subject;

            routeReturn.Should().Match<Route>((x) =>
                    x.Name == route.Name &&
                    x.CreatedAt == route.CreatedAt &&
                    x.AreaId == route.AreaId &&
                    x.Area == route.Area &&   
                    x.Description == route.Description &&
                    x.Location == route.Location &&
                    x.Status == route.Status &&
                    x.Id != Guid.Empty
                );
        }

        [Fact]
        public void ValidateInsert_Message()
        {
            routeRepository.Insert(route).Returns(route);

            unitOfWork.ValidateEntity = true;
            routeService.Invoking(y => y.Insert(route))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        #endregion

        #region Update

        [Fact]
        public void ValidateUpdate_Success()
        {
            route.Id = Guid.NewGuid();
            route.LastUpdatedAt = DateTime.Parse("20/02/2013");

            routeRepository.Update(route).Returns(x =>
            {
                route.LastUpdatedAt = DateTime.UtcNow.Date;
                return route;
            });

            var routeReturn = routeService.Invoking(y => y.Update(route))
                .Should().NotThrow().Subject;

            routeReturn.Should().Match<Route>((x) =>
                    x.Name == route.Name &&
                    x.CreatedAt == route.CreatedAt &&
                    x.AreaId == route.AreaId &&
                    x.Area == route.Area &&
                    x.Description == route.Description &&
                    x.Location == route.Location &&
                    x.Status == route.Status &&
                    x.LastUpdatedAt == DateTime.UtcNow.Date &&
                    x.Id == route.Id
                );
        }

        [Fact]
        public void ValidateUpdate_Message()
        {
            route.Id = Guid.NewGuid();
            route.Name = null;
            routeRepository.Update(route).Returns(route);

            unitOfWork.ValidateEntity = true;
            routeService.Invoking(y => y.Update(route))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        #endregion

        #region CreateGeometry

        private static GeoJSONObject CreatePolygon()
        {
            return new GeoJSON.Net.Geometry.Polygon(new List<GeoJSON.Net.Geometry.LineString>
                {
                    new GeoJSON.Net.Geometry.LineString(new List<GeoJSON.Net.Geometry.Position>
                    {
                        new GeoJSON.Net.Geometry.Position(-43.202877044677734, -19.637575622757698),
                        new GeoJSON.Net.Geometry.Position(-43.20467948913574, -19.6387073583296),
                        new GeoJSON.Net.Geometry.Position(-43.20322036743164, -19.639677411042133),
                        new GeoJSON.Net.Geometry.Position(-43.201847076416016, -19.638788196279503),
                        new GeoJSON.Net.Geometry.Position(-43.202877044677734, -19.637575622757698)

                    })
                });
        }

        private static GeoJSONObject CreateLineString()
        {
            return new GeoJSON.Net.Geometry.LineString(new List<GeoJSON.Net.Geometry.Position>
                    {
                        new GeoJSON.Net.Geometry.Position(-43.202322372018273, -19.634192139453091),
                        new GeoJSON.Net.Geometry.Position(-43.2023795712837, -19.634192710981779),
                        new GeoJSON.Net.Geometry.Position(-43.202379269686112, -19.634219808792981),
                        new GeoJSON.Net.Geometry.Position(-43.202350670048418, -19.634219523030421),
                        new GeoJSON.Net.Geometry.Position(-43.202350066842143, -19.634273718651681),
                        new GeoJSON.Net.Geometry.Position(-43.20229286754892, -19.634273147111291),
                        new GeoJSON.Net.Geometry.Position(-43.202293169161983, -19.63424604930157),
                        new GeoJSON.Net.Geometry.Position(-43.202321768803472, -19.634246335073609),
                        new GeoJSON.Net.Geometry.Position(-43.202322372018273, -19.634192139453091)
                    });

        }

        private static GeoJSONObject CreatePoint()
        {
            return new GeoJSON.Net.Geometry.Point(
                 new GeoJSON.Net.Geometry.Position(-43.193382024765015, -19.64893303587703));
        }

        private static IGeometry MontarGeometry(GeoJSONObject obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            return (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

        }

        #endregion
    }
}

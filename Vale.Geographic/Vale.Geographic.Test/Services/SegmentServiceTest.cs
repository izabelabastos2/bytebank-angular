using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using AutoFixture.AutoNSubstitute;
using System.Collections.Generic;
using System.Globalization;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using FluentAssertions;
using NetTopologySuite;
using Newtonsoft.Json;
using AutoFixture;
using GeoJSON.Net;
using NSubstitute;
using System.Linq;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Services
{
    public class SegmentServiceTest
    {
        private readonly Faker faker;
        private readonly Area area;
        private readonly Route route;
        private readonly Segment segment;
        private readonly Category category;

        private readonly SegmentValidator segmentValidator;
        private readonly SegmentService segmentService;

        private readonly IFixture fixture;
        private readonly IAuditoryService AuditService;
        private readonly ISegmentRepository segmentRepository;
        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;

        private readonly IUnitOfWork unitOfWork;

        public SegmentServiceTest()
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
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Length, (f, u) => f.Random.Double())
                .RuleFor(u => u.Location, MontarGeometry(CreateLineString()))
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


            this.segment = new Faker<Segment>()
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Length, (f, u) => f.Random.Double())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.CreatedBy, (f, u) => f.Random.String())
                .RuleFor(u => u.LastUpdatedBy, (f, u) => f.Random.String())
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Area, area)
                .RuleFor(u => u.RouteId, route.Id)
                .RuleFor(u => u.Route, route)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


            fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.segmentRepository = fixture.Freeze<ISegmentRepository>();
            this.routeRepository = fixture.Freeze<IRouteRepository>();
            this.areaRepository = fixture.Freeze<IAreaRepository>();
            this.AuditService = fixture.Freeze<IAuditoryService>();

            this.unitOfWork = fixture.Freeze<IUnitOfWork>();

            this.segmentService = new SegmentService(unitOfWork, segmentRepository, routeRepository, areaRepository, AuditService);
        }

        #region Insert

        [Fact]
        public void ValidateInsert_Success()
        {
            segmentRepository.Insert(segment).Returns(x =>
            {
                segment.Id = Guid.NewGuid();
                return segment;
            });

            var segmentReturn = segmentService.Invoking(y => y.Insert(segment))
                .Should().NotThrow().Subject;

            segmentReturn.Should().Match<Segment>((x) =>
                    x.Name == segment.Name &&
                    x.CreatedAt == segment.CreatedAt &&
                    x.AreaId == segment.AreaId &&
                    x.Area == segment.Area &&
                    x.RouteId == segment.RouteId &&
                    x.Route == segment.Route &&
                    x.Length == segment.Length &&
                    x.Description == segment.Description &&
                    x.Location == segment.Location &&
                    x.Status == segment.Status &&
                    x.Id != Guid.Empty
                );
        }

        [Fact]
        public void ValidateInsert_Message()
        {
            var segmentReturn = new Segment();
            segmentReturn.Location = MontarGeometry(CreatePolygon());

            segmentRepository.Insert(segmentReturn).Returns(segmentReturn);

            unitOfWork.ValidateEntity = true;

            segmentService.Invoking(y => y.Insert(segmentReturn))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        #endregion

        #region Update

        [Fact]
        public void ValidateUpdate_Success()
        {
            segment.Id = Guid.NewGuid();
            segment.LastUpdatedAt = DateTime.ParseExact("20/02/2013", "dd/MM/yyyy", CultureInfo.CurrentCulture);

            segmentRepository.Update(segment).Returns(x =>
            {
                segment.LastUpdatedAt = DateTime.UtcNow.Date;
                return segment;
            });

            var segmentReturn = segmentService.Invoking(y => y.Update(segment))
                .Should().NotThrow().Subject;

            segmentReturn.Should().Match<Segment>((x) =>
                    x.Name == segment.Name &&
                    x.CreatedAt == segment.CreatedAt &&
                    x.AreaId == segment.AreaId &&
                    x.Area == segment.Area &&
                    x.RouteId == segment.RouteId &&
                    x.Route == segment.Route &&
                    x.Length == segment.Length &&
                    x.Description == segment.Description &&
                    x.Location == segment.Location &&
                    x.Status == segment.Status &&
                    x.LastUpdatedAt == DateTime.UtcNow.Date &&
                    x.Id == segment.Id
                );
        }

        [Fact]
        public void ValidateUpdate_Message()
        {
            segment.Id = Guid.NewGuid();
            segment.Name = null;
            segmentRepository.Update(segment).Returns(segment);

            unitOfWork.ValidateEntity = true;
            segmentService.Invoking(y => y.Update(segment))
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

            return (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json)).Normalized().Reverse();

        }

        #endregion


    }
}

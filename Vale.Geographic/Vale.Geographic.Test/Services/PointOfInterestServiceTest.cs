using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using AutoFixture.AutoNSubstitute;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using FluentAssertions;
using Newtonsoft.Json;
using AutoFixture;
using System.Linq;
using GeoJSON.Net;
using NSubstitute;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Services
{
    public class PointOfInterestServiceTest
    {
        private readonly Faker faker;
        private readonly PointOfInterest pointOfInterest;
        private readonly Category category;
        private readonly Area area;

        private readonly PointOfInterestService pointOfInterestService;
        private readonly PointOfInterestValidator pointOfInterestValidator;

        private readonly IFixture fixture;
        private readonly IAreaRepository areaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IPointOfInterestRepository pointOfInterestRepository;

        private readonly IUnitOfWork unitOfWork;

        public PointOfInterestServiceTest()
        {
            faker = new Faker();

            this.category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.PointOfInterest)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.area = new Faker<Area>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            this.pointOfInterest = new Faker<PointOfInterest>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Area, area)
                .RuleFor(u => u.CategoryId, category.Id)
                .RuleFor(u => u.Category, category)
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Location, MontarGeometry(CreatePoint()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.categoryRepository = fixture.Freeze<ICategoryRepository>();
            this.pointOfInterestRepository = fixture.Freeze<IPointOfInterestRepository>();
            this.areaRepository = fixture.Freeze<IAreaRepository>();
            this.unitOfWork = fixture.Freeze<IUnitOfWork>();

            this.pointOfInterestService = fixture.Create<PointOfInterestService>();
        }

        #region Insert

        [Fact]
        public void ValidateInsert_Success()
        {
            pointOfInterestRepository.Insert(pointOfInterest).Returns(x =>
            {
                pointOfInterest.Id = Guid.NewGuid();
                return pointOfInterest;
            });

            var point = pointOfInterestService.Invoking(y => y.Insert(pointOfInterest))
                .Should().NotThrow().Subject;

            point.Should().Match<PointOfInterest>((x) =>
                    x.Name == pointOfInterest.Name &&
                    x.CreatedAt == pointOfInterest.CreatedAt &&
                    x.AreaId == pointOfInterest.AreaId &&
                    x.Area == pointOfInterest.Area &&
                    x.CategoryId == pointOfInterest.CategoryId &&
                    x.Category == pointOfInterest.Category &&
                    x.Description == pointOfInterest.Description &&
                    x.Location == pointOfInterest.Location &&
                    x.Status == pointOfInterest.Status &&
                    x.Id != Guid.Empty
                );
        }

        [Fact]
        public void ValidateInsert_Message()
        {
            pointOfInterestRepository.Insert(pointOfInterest).Returns(pointOfInterest);

            unitOfWork.ValidateEntity = true;
            pointOfInterestService.Invoking(y => y.Insert(pointOfInterest))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        #endregion

        #region Update

        [Fact]
        public void ValidateUpdate_Success()
        {
            pointOfInterest.Id = Guid.NewGuid();
            pointOfInterest.LastUpdatedAt = DateTime.Parse("20/02/2013");

            pointOfInterestRepository.Update(pointOfInterest).Returns(x =>
            {
                pointOfInterest.LastUpdatedAt = DateTime.UtcNow.Date; 
                return pointOfInterest;
            });

            var point = pointOfInterestService.Invoking(y => y.Update(pointOfInterest))
                .Should().NotThrow().Subject;

            point.Should().Match<PointOfInterest>((x) =>
                    x.Name == pointOfInterest.Name &&
                    x.CreatedAt == pointOfInterest.CreatedAt &&
                    x.AreaId == pointOfInterest.AreaId &&
                    x.Area == pointOfInterest.Area &&
                    x.CategoryId == pointOfInterest.CategoryId &&
                    x.Category == pointOfInterest.Category &&
                    x.Description == pointOfInterest.Description &&
                    x.Location == pointOfInterest.Location &&
                    x.Status == pointOfInterest.Status &&
                    x.LastUpdatedAt == DateTime.UtcNow.Date  &&
                    x.Id == pointOfInterest.Id
                );
        }

        [Fact]
        public void ValidateUpdate_Message()
        {
            pointOfInterest.Id = Guid.NewGuid();
            pointOfInterest.Name = null;
            pointOfInterestRepository.Update(pointOfInterest).Returns(pointOfInterest);

            unitOfWork.ValidateEntity = true;
            pointOfInterestService.Invoking(y => y.Update(pointOfInterest))
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

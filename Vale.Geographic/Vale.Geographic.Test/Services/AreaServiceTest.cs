using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using FluentAssertions;
using Newtonsoft.Json;
using GeoJSON.Net;
using NSubstitute;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Services
{
    public class AreaServiceTest
    {
        private readonly Faker faker;
        private readonly Area area;
        private readonly Category category;


        private readonly AreaService areaService;
        private readonly AreaValidator areaValidator;

        private readonly IAreaRepository areaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public AreaServiceTest()
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
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.CategoryId, category.Id)
                .RuleFor(u => u.Category, category)
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            this.categoryRepository = Substitute.For<ICategoryRepository>();
            this.areaRepository = Substitute.For<IAreaRepository>();
            this.unitOfWork = Substitute.For<IUnitOfWork>();

            this.areaService = new AreaService(unitOfWork, areaRepository, categoryRepository);
        }


        #region Insert

        [Fact]
        public void ValidateInsert_Success()
        {
            areaRepository.Insert(area).Returns(x =>
            {
                area.Id = Guid.NewGuid();
                return area;
            });

            categoryRepository.GetById(area.CategoryId.Value).Returns(category);

            var areaReturn = areaService.Invoking(y => y.Insert(area))
                .Should().NotThrow().Subject;

            areaReturn.Should().Match<Area>((x) =>
                    x.Name == area.Name &&
                    x.CreatedAt == area.CreatedAt &&
                    x.CategoryId == area.CategoryId &&
                    x.Category == area.Category &&
                    x.Description == area.Description &&
                    x.Location == area.Location &&
                    x.Status == area.Status &&
                    x.Id != Guid.Empty
                );
        }

        [Fact]
        public void ValidateInsert_Message()
        {
            area.Name = null;
            areaRepository.Insert(area).Returns(area);

            unitOfWork.ValidateEntity = true;
            areaService.Invoking(y => y.Insert(area))
                .Should().Throw<FluentValidation.ValidationException>();

        }

        #endregion

        #region Update

        [Fact]
        public void ValidateUpdate_Success()
        {
            area.Id = Guid.NewGuid();
            area.LastUpdatedAt = DateTime.Parse("20/02/2013");


            areaRepository.Update(area).Returns(x =>
            {
                area.LastUpdatedAt = DateTime.UtcNow.Date;
                return area;
            });

            var areaReturn = areaService.Invoking(y => y.Update(area))
                .Should().NotThrow().Subject;

            areaReturn.Should().Match<Area>((x) =>
                    x.Name == area.Name &&
                    x.CreatedAt == area.CreatedAt &&
                    x.CategoryId == area.CategoryId &&
                    x.Category == area.Category &&
                    x.Description == area.Description &&
                    x.Location == area.Location &&
                    x.Status == area.Status &&
                    x.LastUpdatedAt == DateTime.UtcNow.Date &&
                    x.Id == area.Id
                );
        }

        [Fact]
        public void ValidateUpdate_Message()
        {
            area.Id = Guid.NewGuid();
            area.Name = null;
            areaRepository.Update(area).Returns(area);

            unitOfWork.ValidateEntity = true;
            areaService.Invoking(y => y.Update(area))
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

        private static IGeometry MontarGeometry(GeoJSONObject obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            return (Geometry)geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

        }

        #endregion
    }
}

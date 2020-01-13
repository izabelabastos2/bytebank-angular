using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using FluentValidation.TestHelper;
using AutoFixture.AutoNSubstitute;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using Newtonsoft.Json;
using AutoFixture;
using GeoJSON.Net;
using System.Linq;
using NSubstitute;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Validations
{
    public class PointOfInterestValidationTest
    {
        private readonly Faker faker;
        private readonly PointOfInterest pointOfInterest;
        private readonly Category category;
        private readonly Area area;

        private readonly IFixture fixture;
        private readonly IPointOfInterestRepository pointOfInterestRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAreaRepository areaRepository;
        private readonly PointOfInterestValidator validator;

        public PointOfInterestValidationTest()
        {
            this.faker = new Faker("en");

            this.area = new Faker<Area>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


            this.pointOfInterest = new Faker<PointOfInterest>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Location, MontarGeometry(CreatePoint()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


            this.category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.PointOfInterest)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
                 fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.categoryRepository = fixture.Freeze<ICategoryRepository>();
            this.pointOfInterestRepository = fixture.Freeze<IPointOfInterestRepository>();
            this.areaRepository = fixture.Freeze<IAreaRepository>();


            this.validator = new PointOfInterestValidator(pointOfInterestRepository, areaRepository, categoryRepository);
        }

        public static readonly List<object[]> ValidId = new List<object[]>
        {
            new object[]{ Guid.NewGuid() },
            new object[]{ Guid.Empty },

        };

        public static readonly List<object[]> Id = new List<object[]>
        {
            new object[]{ Guid.NewGuid() },
            new object[]{ null },
        };

        public static readonly List<object[]> InvalidId = new List<object[]>
        {
            new object[]{ Guid.Empty },
            new object[]{ null },
        };

        public static readonly List<object[]> IncorrectData = new List<object[]>
        {
            new object[]{ DateTime.MinValue },
            new object[]{ null }
        };

        #region Id

        [Fact]
        public void ValidateId_Id_Sucesso()
        {
            this.pointOfInterest.Id = Guid.NewGuid();

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, pointOfInterest);
        }

        [Theory]
        [MemberData(nameof(InvalidId))]
        public void ValidateId_EmptyOrInvalid_Message(Guid id)
        {
            this.pointOfInterest.Id = id;

            validator.ShouldHaveValidationErrorFor(x => x.Id, pointOfInterest)
              .WithErrorMessage(Domain.Resources.Validations.PointOfInterestIdRequired);
        }

        #endregion

        #region CategoryId

        [Theory]
        [MemberData(nameof(Id))]
        public void ValidateCategoryId_CategoryIdOrNull_Sucesso(Guid id)
        {
            this.pointOfInterest.CategoryId = id;

            if (id != null)
            {
                var categoryRetorno = new Faker<Category>()
                   .RuleFor(u => u.Id, pointOfInterest.CategoryId)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

                categoryRepository.GetById(pointOfInterest.CategoryId.Value).Returns(categoryRetorno);
            }

            validator.ShouldNotHaveValidationErrorFor(x => x.CategoryId, pointOfInterest);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateCategoryId_NotFound_Message(Guid id)
        {
            this.pointOfInterest.CategoryId = id;

            categoryRepository.Get(pointOfInterest.CategoryId.Value, out int total, true).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.CategoryId, pointOfInterest)
                .WithErrorMessage(Domain.Resources.Validations.CategoryNotFound);
        }

        #endregion

        #region Category

        [Fact]
        public void ValidateCategory_Category_Sucesso()
        {
            this.pointOfInterest.Category = category;

            validator.ShouldNotHaveValidationErrorFor(x => x.Category, pointOfInterest);
        }


        [Fact]
        public void ValidateCategory_CategoryInvalid_Message()
        {
            this.pointOfInterest.Category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.Area)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.pointOfInterest.CategoryId = category.Id;


            validator.ShouldHaveValidationErrorFor(x => x.Category, pointOfInterest)
             .WithErrorMessage(Domain.Resources.Validations.PointOfInterestCategoryInvalid);
        }

        #endregion

        #region AreaId

        [Fact]
        public void ValidateAreaId_AreaIdOrNull_Sucesso()
        {
            this.pointOfInterest.AreaId = Guid.NewGuid();
            
            var areaRetorno = new Faker<Area>()
                .RuleFor(u => u.Id, pointOfInterest.AreaId)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            this.areaRepository.GetById(pointOfInterest.AreaId).Returns(areaRetorno);            

            validator.ShouldNotHaveValidationErrorFor(x => x.AreaId, pointOfInterest);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateAreaId_NotFound_Message(Guid id)
        {
            this.pointOfInterest.AreaId = id;

            areaRepository.GetById(pointOfInterest.AreaId).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.AreaId, pointOfInterest)
              .WithErrorMessage(id == Guid.Empty ?
                 Domain.Resources.Validations.PointOfInterestAreaIdRequired :
                 Domain.Resources.Validations.AreaNotFound);
        }

        #endregion

        #region Description

        [Fact]
        public void ValidateDescription_DescriptionGreaterThan255_Message()
        {
            this.pointOfInterest.Description = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Description, pointOfInterest)
                .WithErrorMessage(Domain.Resources.Validations.PointOfInterestDescriptionLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData(null)]
        public void ValidateDescription_Description_Sucesso(string description)
        {
            this.pointOfInterest.Description = description;
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, pointOfInterest);
        }

        #endregion

        #region Name

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateName_NameNullEmpty_Message(string name)
        {
            this.pointOfInterest.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, pointOfInterest)
             .WithErrorMessage(Domain.Resources.Validations.PointOfInterestNameRequired);

        }

        [Fact]
        public void ValidateName_NameGreaterThan255_Message()
        {
            this.pointOfInterest.Name = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Name, pointOfInterest)
                .WithErrorMessage(Domain.Resources.Validations.PointOfInterestNameLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData("teste")]
        public void ValidateName_Name_Sucesso(string name)
        {
            this.pointOfInterest.Name = name;
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, pointOfInterest);
        }

        #endregion

        #region CreatedAt

        [Theory]
        [MemberData(nameof(IncorrectData))]
        public void ValidateCreatedAt_CreatedAtMinValue_Message(DateTime createdAt)
        {
            this.pointOfInterest.CreatedAt = createdAt;

            validator.ShouldHaveValidationErrorFor(x => x.CreatedAt, pointOfInterest)
             .WithErrorMessage(Domain.Resources.Validations.PointOfInterestCreatedAtRequired);

        }

        [Fact]
        public void ValidateCreatedAt_CreatedAt_Sucesso()
        {
            this.pointOfInterest.CreatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.CreatedAt, pointOfInterest);
        }

        #endregion

        #region LastUpdatedAt

        [Theory]
        [MemberData(nameof(IncorrectData))]
        public void ValidateLastUpdatedAt_LastUpdatedAtMinValue_Message(DateTime lastUpdatedAt )
        {
            this.pointOfInterest.LastUpdatedAt = lastUpdatedAt;

            validator.ShouldHaveValidationErrorFor(x => x.LastUpdatedAt, pointOfInterest)
             .WithErrorMessage(Domain.Resources.Validations.PointOfInterestLastUpdatedAtRequired);
        }

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAt_Sucesso()
        {
            this.pointOfInterest.LastUpdatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.LastUpdatedAt, pointOfInterest);
        }

        #endregion

        #region Status

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidateStatus_Status_Sucesso(bool status)
        {
            this.pointOfInterest.Status = status;
            validator.ShouldNotHaveValidationErrorFor(x => x.Status, pointOfInterest);
        }

        #endregion

        #region Location

        public static readonly List<object[]> ValidGeometry = new List<object[]>
        {
            new object[]{ MontarGeometry(CreatePolygon()) },
            new object[]{ MontarGeometry(CreateMultiPolygon()) },
        };

        [Fact]
        public void ValidateLocation_Location_Sucesso()
        {
            this.pointOfInterest.Location = MontarGeometry(CreatePoint());
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, pointOfInterest);
        }

        [Theory]
        [MemberData(nameof(ValidGeometry))]
        public void ValidateLocation_LocationInvalid_Message(IGeometry geometry)
        {
            this.pointOfInterest.Location = geometry;

            validator.ShouldHaveValidationErrorFor(x => x.Location, pointOfInterest)
             .WithErrorMessage(Domain.Resources.Validations.PointOfInterestLocationInvalid);
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

        private static GeoJSONObject CreateMultiPolygon()
        {
            return new GeoJSON.Net.Geometry.MultiPolygon(new List<GeoJSON.Net.Geometry.Polygon>
                {
                    new GeoJSON.Net.Geometry.Polygon(new List<GeoJSON.Net.Geometry.LineString>
                    {
                       new GeoJSON.Net.Geometry.LineString(new List<GeoJSON.Net.Geometry.Position>
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

                       })
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

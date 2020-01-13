using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using FluentValidation.TestHelper;
using AutoFixture.AutoNSubstitute;
using System.Collections.Generic;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using Newtonsoft.Json;
using GeoJSON.Net;
using NSubstitute;
using AutoFixture;
using System.Linq;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Validations
{
    public class RouteValidationTest
    {
        private readonly Faker faker;
        private readonly Route route;
        private readonly Area area;

        private readonly IFixture fixture;
        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;
        private readonly RouteValidator validator;


        public RouteValidationTest()
        {
            this.faker = new Faker("en");

            this.area = new Faker<Area>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());         

            this.route = new Faker<Route>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Location, MontarGeometry(CreateLineString()))
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.routeRepository = fixture.Freeze<IRouteRepository>();
            this.areaRepository = fixture.Freeze<IAreaRepository>();

            this.validator = new RouteValidator(routeRepository, areaRepository);
        }

        public static readonly List<object[]> ValidId = new List<object[]>
        {
            new object[]{ Guid.NewGuid() },
            new object[]{ Guid.Empty },

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
            this.route.Id = Guid.NewGuid();

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, route);
        }

        [Theory]
        [MemberData(nameof(InvalidId))]
        public void ValidateId_EmptyOrInvalid_Message(Guid id)
        {
            this.route.Id = id;

            validator.ShouldHaveValidationErrorFor(x => x.Id, route)
              .WithErrorMessage(Domain.Resources.Validations.RouteIdRequired);
        }

        #endregion

        #region AreaId

        [Fact]
        public void ValidateAreaId_AreaIdOrNull_Sucesso()
        {
            this.route.AreaId = Guid.NewGuid();

            var areaRetorno = new Faker<Area>()
                .RuleFor(u => u.Id, route.AreaId)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            areaRepository.GetById(route.AreaId).Returns(areaRetorno);

            validator.ShouldNotHaveValidationErrorFor(x => x.AreaId, route);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateAreaId_NotFound_Message(Guid id)
        {
            this.route.AreaId = id;

            areaRepository.GetById(route.AreaId).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.AreaId, route)
              .WithErrorMessage(id == Guid.Empty ?
                 Domain.Resources.Validations.RouteAreaIdRequired :
                 Domain.Resources.Validations.AreaNotFound);
        }

        #endregion

        #region Description

        [Fact]
        public void ValidateDescription_DescriptionGreaterThan255_Message()
        {
            this.route.Description = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Description, route)
                .WithErrorMessage(Domain.Resources.Validations.RouteDescriptionLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData(null)]
        public void ValidateDescription_Description_Sucesso(string description)
        {
            this.route.Description = description;
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, route);
        }

        #endregion

        #region Name

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateName_NameNullEmpty_Message(string name)
        {
            this.route.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, route)
             .WithErrorMessage(Domain.Resources.Validations.RouteNameRequired);

        }

        [Fact]
        public void ValidateName_NameGreaterThan255_Message()
        {
            this.route.Name = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Name, route)
                .WithErrorMessage(Domain.Resources.Validations.RouteNameLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData("teste")]
        public void ValidateName_Name_Sucesso(string name)
        {
            this.route.Name = name;
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, route);
        }

        #endregion

        #region CreatedAt

        [Theory]
        [MemberData(nameof(IncorrectData))]
        public void ValidateCreatedAt_CreatedAtMinValue_Message(DateTime createdAt)
        {
            this.route.CreatedAt = createdAt;

            validator.ShouldHaveValidationErrorFor(x => x.CreatedAt, route)
             .WithErrorMessage(Domain.Resources.Validations.RouteCreatedAtRequired);

        }

        [Fact]
        public void ValidateCreatedAt_CreatedAt_Sucesso()
        {
            this.route.CreatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.CreatedAt, route);
        }

        #endregion

        #region LastUpdatedAt

        [Theory]
        [MemberData(nameof(IncorrectData))]
        public void ValidateLastUpdatedAt_LastUpdatedAtMinValue_Message(DateTime lastUpdatedAt)
        {
            this.route.LastUpdatedAt = lastUpdatedAt;

            validator.ShouldHaveValidationErrorFor(x => x.LastUpdatedAt, route)
             .WithErrorMessage(Domain.Resources.Validations.RouteLastUpdatedAtRequired);

        }

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAt_Sucesso()
        {
            this.route.LastUpdatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.LastUpdatedAt, route);
        }

        #endregion

        #region Status

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidateStatus_Status_Sucesso(bool status)
        {
            this.route.Status = status;
            validator.ShouldNotHaveValidationErrorFor(x => x.Status, route);
        }

        #endregion

        #region Location

        [Fact]
        public void ValidateLocation_Location_Sucesso()
        {
            this.route.Location = MontarGeometry(CreateLineString());
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, route);
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

        private static IGeometry MontarGeometry(GeoJSONObject obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            return geometryFactory.CreateGeometry(new GeoJsonReader().Read<Geometry>(json));

        }

        #endregion
    }
}

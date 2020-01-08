using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite;
using Newtonsoft.Json;
using GeoJSON.Net;
using NSubstitute;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Validations
{
    public class SegmentValidatonTest
    {
        private readonly Faker faker;
        private readonly Route route;
        private readonly Area area;
        private readonly Segment segment;

        private readonly IRouteRepository routeRepository;
        private readonly IAreaRepository areaRepository;
        private readonly ISegmentRepository segmentRepository;

        private readonly SegmentValidator validator;

        public SegmentValidatonTest()
        {
            this.faker = new Faker("en");

            this.route = new Faker<Route>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Location, MontarGeometry(CreateLineString()))
                .RuleFor(u => u.AreaId, Guid.NewGuid())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.area = new Faker<Area>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreateMultiPolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            this.segment = new Faker<Segment>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.AreaId, area.Id)
                .RuleFor(u => u.RouteId, route.Id)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.segmentRepository = Substitute.For<ISegmentRepository>();
            this.routeRepository = Substitute.For<IRouteRepository>();
            this.areaRepository = Substitute.For<IAreaRepository>();

            this.validator = new SegmentValidator(segmentRepository, routeRepository, areaRepository);
        }

        public static readonly List<object[]> ValidId = new List<object[]>
        {
            new object[]{ Guid.NewGuid() },
            new object[]{ Guid.Empty },

        };

        #region Id

        [Fact]
        public void ValidateId_Id_Sucesso()
        {
            this.segment.Id = Guid.NewGuid();

            var segmentReturn = new Faker<Segment>()
                   .RuleFor(u => u.Id, segment.Id)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                   .RuleFor(u => u.AreaId, area.Id)
                   .RuleFor(u => u.RouteId, route.Id)
                   .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                   .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            //segmentRepository.GetById(segment.Id).Returns(segmentReturn);
            segmentRepository.RecoverById(segment.Id).Returns(segmentReturn);

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, segment);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateId_EmptyOrInvalid_Message(Guid id)
        {
            this.segment.Id = id;
            //segmentRepository.GetById(segment.Id).Returns(x => null);
            segmentRepository.RecoverById(segment.Id).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.Id, segment)
              .WithErrorMessage(id == Guid.Empty || id == null ?
                 Domain.Resources.Validations.SegmentIdRequired :
                 Domain.Resources.Validations.SegmentNotFound);
        }

        #endregion

        #region AreaId

        [Fact]
        public void ValidateAreaId_AreaId_Sucesso()
        {
            this.segment.AreaId = Guid.NewGuid();

            var areaRetorno = new Faker<Area>()
                .RuleFor(u => u.Id, segment.AreaId)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            areaRepository.GetById(segment.AreaId).Returns(areaRetorno);

            validator.ShouldNotHaveValidationErrorFor(x => x.AreaId, segment);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateAreaId_NotFound_Message(Guid id)
        {
            this.segment.AreaId = id;

            areaRepository.GetById(segment.AreaId).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.AreaId, segment)
              .WithErrorMessage(id == Guid.Empty ?
                 Domain.Resources.Validations.SegmentAreaIdRequired :
                 Domain.Resources.Validations.AreaNotFound);
        }

        #endregion

        #region RouteId

        [Fact]
        public void ValidateRouteId_RouteId_Sucesso()
        {
            this.segment.RouteId = Guid.NewGuid();

            var routeRetorno = new Faker<Route>()
                .RuleFor(u => u.Id, segment.RouteId)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreateLineString()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            routeRepository.GetById(segment.RouteId).Returns(routeRetorno);

            validator.ShouldNotHaveValidationErrorFor(x => x.RouteId, segment);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateRouteId_NotFound_Message(Guid id)
        {
            this.segment.RouteId = id;

            routeRepository.GetById(segment.RouteId).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.RouteId, segment)
              .WithErrorMessage(id == Guid.Empty ?
                 Domain.Resources.Validations.SegmentRouteIdRequired :
                 Domain.Resources.Validations.RouteNotFound);
        }

        #endregion

        #region Description

        [Fact]
        public void ValidateDescription_DescriptionGreaterThan255_Message()
        {
            this.segment.Description = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Description, segment)
                .WithErrorMessage(Domain.Resources.Validations.SegmentDescriptionLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData(null)]
        public void ValidateDescription_Description_Sucesso(string description)
        {
            this.segment.Description = description;
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, segment);
        }

        #endregion

        #region Name

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateName_NameNullEmpty_Message(string name)
        {
            this.segment.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, segment)
             .WithErrorMessage(Domain.Resources.Validations.SegmentNameRequired);

        }

        [Fact]
        public void ValidateName_NameGreaterThan255_Message()
        {
            this.segment.Name = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Name, segment)
                .WithErrorMessage(Domain.Resources.Validations.SegmentNameLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData("teste")]
        public void ValidateName_Name_Sucesso(string name)
        {
            this.segment.Name = name;
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, segment);
        }

        #endregion

        #region CreatedAt

        [Fact]
        public void ValidateCreatedAt_CreatedAtMinValue_Message()
        {
            this.segment.CreatedAt = DateTime.MinValue;

            validator.ShouldHaveValidationErrorFor(x => x.CreatedAt, segment)
             .WithErrorMessage(Domain.Resources.Validations.SegmentCreatedAtRequired);

        }

        [Fact]
        public void ValidateCreatedAt_CreatedAt_Sucesso()
        {
            this.segment.CreatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.CreatedAt, segment);
        }

        #endregion

        #region LastUpdatedAt

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAtMinValue_Message()
        {
            this.segment.LastUpdatedAt = DateTime.MinValue;

            validator.ShouldHaveValidationErrorFor(x => x.LastUpdatedAt, segment)
             .WithErrorMessage(Domain.Resources.Validations.SegmentLastUpdatedAtRequired);

        }

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAt_Sucesso()
        {
            this.segment.LastUpdatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.LastUpdatedAt, segment);
        }

        #endregion

        #region Status

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidateStatus_Status_Sucesso(bool status)
        {
            this.segment.Status = status;
            validator.ShouldNotHaveValidationErrorFor(x => x.Status, segment);
        }

        #endregion

        #region Location

        [Fact]
        public void ValidateLocation_Location_Sucesso()
        {
            this.segment.Location = MontarGeometry(CreatePolygon());
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, segment);
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

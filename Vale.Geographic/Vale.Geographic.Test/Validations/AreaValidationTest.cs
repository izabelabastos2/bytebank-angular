using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using NetTopologySuite.Geometries;
using FluentValidation.TestHelper;
using System.Collections.Generic;
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
    public class AreaValidationTest
    {
        private readonly Faker faker;
        private readonly Category category;
        private readonly Area area;

        private readonly IAreaRepository areaRepository;
        private readonly ICategoryRepository categoryRepository;

        private readonly AreaValidator validator;

        public AreaValidationTest()
        {
            this.faker = new Faker("en");

            this.area = new Faker<Area>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
                .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


            this.category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.Area)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.categoryRepository = Substitute.For<ICategoryRepository>();
            this.areaRepository = Substitute.For<IAreaRepository>();

            this.validator = new AreaValidator(areaRepository, categoryRepository);
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


        #region Id

        [Fact]
        public void ValidateId_Id_Sucesso()
        {

            area.Id = Guid.NewGuid();

            var areaRetorno = new Faker<Area>()
                   .RuleFor(u => u.Id, area.Id)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                   .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

            //areaRepository.GetById(area.Id).Returns(areaRetorno);
            areaRepository.RecoverById(area.Id).Returns(areaRetorno);

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, area);

        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateId_EmptyOrInvalid_Message(Guid id)
        {
            area.Id = id;
            //areaRepository.GetById(area.Id).Returns(x => null);
            areaRepository.RecoverById(area.Id).Returns(x => null);


            validator.ShouldHaveValidationErrorFor(x => x.Id, area)
              .WithErrorMessage(id == Guid.Empty || id == null ?
                 Domain.Resources.Validations.AreaIdRequired :
                 Domain.Resources.Validations.AreaNotFound);
        }

        #endregion

        #region CategoryId

        [Theory]
        [MemberData(nameof(Id))]
        public void ValidateCategoryId_CategoryIdOrNull_Sucesso(Guid? id)
        {
            area.CategoryId = id;

            if (id != null)
            {
                var categoryRetorno = new Faker<Category>()
                   .RuleFor(u => u.Id, area.CategoryId)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

                categoryRepository.GetById(area.CategoryId.Value).Returns(categoryRetorno);
            }

            validator.ShouldNotHaveValidationErrorFor(x => x.CategoryId, area);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateCategoryId_NotFound_Message(Guid id)
        {
            area.CategoryId = id;

            categoryRepository.Get(area.CategoryId.Value, out int total, true).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.CategoryId, area)
                .WithErrorMessage( Domain.Resources.Validations.CategoryNotFound);
        }

        #endregion

        #region Category

        [Fact]
        public void ValidateCategory_Category_Sucesso()
        {
            area.Category = category;

            validator.ShouldNotHaveValidationErrorFor(x => x.Category, area);
        }


        [Fact]
        public void ValidateCategory_CategoryInvalid_Message()
        {
            area.Category =  new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
                .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.PointOfInterest)
                .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());


            validator.ShouldHaveValidationErrorFor(x => x.Category, area)
             .WithErrorMessage(Domain.Resources.Validations.AreaCategoryInvalid);
        }

        #endregion

        #region ParentId

        [Theory]
        [MemberData(nameof(Id))]
        public void ValidateParentId_ParentIdOrNull_Sucesso(Guid? id)
        {
            area.ParentId = id;

            if (id != null)
            {
                var areaRetorno = new Faker<Area>()
                   .RuleFor(u => u.Id, area.ParentId)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                   .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

                //areaRepository.GetById(area.ParentId.Value).Returns(areaRetorno);
                areaRepository.RecoverById(area.ParentId.Value).Returns(areaRetorno);
            }

            validator.ShouldNotHaveValidationErrorFor(x => x.ParentId, area);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateParentId_NotFound_Message(Guid id)
        {
            area.ParentId = id;

            //areaRepository.GetById(area.ParentId.Value).Returns(x => null);
            areaRepository.RecoverById(area.ParentId.Value).Returns(x => null);

            validator.ShouldHaveValidationErrorFor(x => x.ParentId.Value, area)
                .WithErrorMessage(Domain.Resources.Validations.AreaNotFound);
        }

        #endregion

        //#region Parent

        //[Fact]
        //public void ValidateParent_Parent_Sucesso()
        //{
        //    area.ParentId = Guid.NewGuid();
        //    area.Parent = new Faker<Area>()
        //        .RuleFor(u => u.Id, area.ParentId)
        //        .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
        //        .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
        //        .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
        //        .RuleFor(u => u.CategoryId, category.Id)
        //        .RuleFor(u => u.Category, category)
        //        .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
        //        .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
        //        .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());

        //    areaRepository.GetById(area.ParentId.Value).Returns(area.Parent);

        //    validator.ShouldNotHaveValidationErrorFor(x => x.Parent, area);
        //}


        //[Fact]
        //public void ValidateParent_ParentInvalid_Message()
        //{
        //    area.Parent = new Faker<Area>()
        //        .RuleFor(u => u.Id, area.ParentId)
        //        .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
        //        .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
        //        .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
        //        .RuleFor(u => u.CategoryId, category.Id)
        //        .RuleFor(u => u.Category, category)
        //        .RuleFor(u => u.Location, MontarGeometry(CreatePolygon()))
        //        .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
        //        .RuleFor(u => u.Description, (f, u) => f.Name.JobDescriptor());


        //    validator.ShouldHaveValidationErrorFor(x => x.Parent, area)
        //     .WithErrorMessage(Domain.Resources.Validations.AreaNotFound);
        //}

        //#endregion

        #region Description

        [Fact]
        public void ValidateDescription_DescriptionGreaterThan255_Message()
        {
            area.Description = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Description, area)
                .WithErrorMessage(Domain.Resources.Validations.AreaDescriptionLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData(null)]
        public void ValidateDescription_Description_Sucesso(string description)
        {
            area.Description = description;
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, area);
        }

        #endregion

        #region Name

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateName_NameNullEmpty_Message(string name)
        {
            area.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, area)
             .WithErrorMessage(Domain.Resources.Validations.AreaNameRequired);

        }

        [Fact]
        public void ValidateName_NameGreaterThan255_Message()
        {
            area.Name = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Name, area)
                .WithErrorMessage(Domain.Resources.Validations.AreaNameLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData("teste")]
        public void ValidateName_Name_Sucesso(string name)
        {
            area.Name = name;
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, area);
        }

        #endregion

        #region CreatedAt

        [Fact]
        public void ValidateCreatedAt_CreatedAtMinValue_Message()
        {
            area.CreatedAt = DateTime.MinValue;

            validator.ShouldHaveValidationErrorFor(x => x.CreatedAt, area)
             .WithErrorMessage(Domain.Resources.Validations.AreaCreatedAtRequired);

        }

        [Fact]
        public void ValidateCreatedAt_CreatedAt_Sucesso()
        {
            area.CreatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.CreatedAt, area);
        }

        #endregion

        #region LastUpdatedAt

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAtMinValue_Message()
        {
            area.LastUpdatedAt = DateTime.MinValue; 

            validator.ShouldHaveValidationErrorFor(x => x.LastUpdatedAt, area)
             .WithErrorMessage(Domain.Resources.Validations.AreaLastUpdatedAtRequired);

        }

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAt_Sucesso()
        {
            area.LastUpdatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.LastUpdatedAt, area);
        }

        #endregion

        #region Status

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidateStatus_Status_Sucesso(bool status)
        {
            area.Status = status;
            validator.ShouldNotHaveValidationErrorFor(x => x.Status, area);
        }

        #endregion

        #region Location

        public static readonly List<object[]> ValidGeometry = new List<object[]>
        {
            new object[]{ MontarGeometry(CreatePolygon()) },
            new object[]{ MontarGeometry(CreateMultiPolygon()) },
        };

        [Theory]
        [MemberData(nameof(ValidGeometry))]
        public void ValidateLocation_Location_Sucesso(IGeometry geometry)
        {           
            area.Location = geometry;
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, area);
        }

        [Fact]
        public void ValidateLocation_LocationInvalid_Message()
        {
            area.Location = MontarGeometry(CreatePoint());

            validator.ShouldHaveValidationErrorFor(x => x.Location, area)
             .WithErrorMessage(Domain.Resources.Validations.AreaLocationInvalid);
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

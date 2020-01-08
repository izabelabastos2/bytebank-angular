using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Entities;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using NSubstitute;
using System;
using Bogus;
using Xunit;

namespace Vale.Geographic.Test.Validations
{
    public class CategoryValidationTest
    {
        private readonly Faker faker;
        private readonly Category category;

        private readonly ICategoryRepository categoryRepository;

        private readonly CategoryValidator validator;

        public CategoryValidationTest()
        {
            this.faker = new Faker("en");

            this.category = new Faker<Category>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.categoryRepository = Substitute.For<ICategoryRepository>();

            this.validator = new CategoryValidator(categoryRepository);
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
            category.Id = Guid.NewGuid();

            var categoryRetorno = new Faker<Category>()
                   .RuleFor(u => u.Id, category.Id)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            //categoryRepository.GetById(category.Id).Returns(categoryRetorno);
            categoryRepository.RecoverById(category.Id).Returns(categoryRetorno);

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, category);
        }

        [Theory]
        [MemberData(nameof(ValidId))]
        public void ValidateId_EmptyOrInvalid_Message(Guid id)
        {
            category.Id = id;
           // categoryRepository.GetById(category.Id).Returns(x => null);
            categoryRepository.RecoverById(category.Id).Returns(x => null);


            validator.ShouldHaveValidationErrorFor(x => x.Id, category)
              .WithErrorMessage(id == Guid.Empty ?
                 Domain.Resources.Validations.CategoryIdRequired :
                 Domain.Resources.Validations.CategoryNotFound);
        }

        #endregion

        #region Name

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateName_NameNullEmpty_Message(string name)
        {
            category.Name = name;
            validator.ShouldHaveValidationErrorFor(x => x.Name, category)
             .WithErrorMessage(Domain.Resources.Validations.CategoryNameRequired);

        }

        [Fact]
        public void ValidateName_NameGreaterThan255_Message()
        {
            category.Name = faker.Random.String(256);
            validator.ShouldHaveValidationErrorFor(x => x.Name, category)
                .WithErrorMessage(Domain.Resources.Validations.CategoryNameLength);
        }

        [Theory]
        [InlineData("TESTE")]
        [InlineData("teste")]
        public void ValidateName_Name_Sucesso(string name)
        {
            category.Name = name;
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, category);
        }

        #endregion

        #region CreatedAt

        [Fact]
        public void ValidateCreatedAt_CreatedAtMinValue_Message()
        {
            category.CreatedAt = DateTime.MinValue;

            validator.ShouldHaveValidationErrorFor(x => x.CreatedAt, category)
             .WithErrorMessage(Domain.Resources.Validations.CategoryCreatedAtRequired);

        }

        [Fact]
        public void ValidateCreatedAt_CreatedAt_Sucesso()
        {
            category.CreatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.CreatedAt, category);
        }

        #endregion

        #region LastUpdatedAt

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAtMinValue_Message()
        {
            category.LastUpdatedAt = DateTime.MinValue;

            validator.ShouldHaveValidationErrorFor(x => x.LastUpdatedAt, category)
             .WithErrorMessage(Domain.Resources.Validations.CategoryLastUpdatedAtRequired);

        }

        [Fact]
        public void ValidateLastUpdatedAt_LastUpdatedAt_Sucesso()
        {
            category.LastUpdatedAt = DateTime.Now;
            validator.ShouldNotHaveValidationErrorFor(x => x.LastUpdatedAt, category);
        }

        #endregion

        #region Status

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidateStatus_Status_Sucesso(bool status)
        {
            category.Status = status;
            validator.ShouldNotHaveValidationErrorFor(x => x.Status, category);
        }

        #endregion

        #region Type

        [Theory]
        [InlineData(TypeEntitieEnum.Area)]
        [InlineData(TypeEntitieEnum.PointOfInterest)]
        public void ValidateType_Type_Sucesso(TypeEntitieEnum type)
        {
            category.TypeEntitie = type;
            validator.ShouldNotHaveValidationErrorFor(x => x.TypeEntitie, category);
        }

        [Fact]
        public void ValidateType_TypeNullEmpty_Message()
        {            
            category.TypeEntitie = 0;
            validator.ShouldHaveValidationErrorFor(x => x.TypeEntitie, category)
               .WithErrorMessage(Domain.Resources.Validations.CategoryTypeRequired);
        }

        #endregion

    }
}

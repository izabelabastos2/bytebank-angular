using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Core.Validations;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Core.Services;
using NSubstitute;
using Bogus;
using Vale.Geographic.Domain.Entities;
using System;
using Xunit;
using FluentAssertions;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Test.Services
{
    public class CategoryServiceTest
    {
        private readonly Faker faker;
        private readonly Category category;

        private readonly CategoryService categoryService;
        private readonly CategoryValidator categoryValidator;

        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoryServiceTest()
        {
            faker = new Faker();

            this.category = new Faker<Category>()
            .RuleFor(u => u.Status, (f, u) => f.Random.Bool())
            .RuleFor(u => u.CreatedAt, DateTime.UtcNow.Date)
            .RuleFor(u => u.TypeEntitie, TypeEntitieEnum.Area)
            .RuleFor(u => u.LastUpdatedAt, DateTime.UtcNow.Date)
            .RuleFor(u => u.Name, (f, u) => f.Name.FullName());

            this.categoryRepository = Substitute.For<ICategoryRepository>();
            this.unitOfWork = Substitute.For<IUnitOfWork>();

            this.categoryService = new CategoryService(unitOfWork, categoryRepository);
        }

        #region Insert

        [Fact]
        public void ValidateInsert_Success()
        {
            categoryRepository.Insert(category).Returns(x =>
            {
                category.Id = Guid.NewGuid();
                return category;
            });

            var categoryReturn = categoryService.Invoking(y => y.Insert(category))
                .Should().NotThrow().Subject;

            categoryReturn.Should().Match<Category>((x) =>
                    x.Name == category.Name &&
                    x.CreatedAt == category.CreatedAt &&
                    x.LastUpdatedAt == category.LastUpdatedAt &&
                    x.TypeEntitie == category.TypeEntitie &&
                    x.Status == category.Status &&
                    x.Id != Guid.Empty
                );
        }

        [Fact]
        public void ValidateInsert_Message()
        {
            category.Name = null;
            categoryRepository.Insert(category).Returns(category);

            unitOfWork.ValidateEntity = true;
            categoryService.Invoking(y => y.Insert(category))
                .Should().Throw<FluentValidation.ValidationException>();

        }
       
        #endregion

        #region Update

        [Fact]
        public void ValidateUpdate_Success()
        {
            category.Id = Guid.NewGuid();
            category.LastUpdatedAt = DateTime.Parse("20/02/2013");

            categoryRepository.Update(category).Returns(x =>
            {
                category.LastUpdatedAt = DateTime.UtcNow.Date;
                return category;
            });

            var categoryReturn = categoryService.Invoking(y => y.Update(category))
                .Should().NotThrow().Subject;

            categoryReturn.Should().Match<Category>((x) =>
                    x.Name == category.Name &&
                    x.CreatedAt == category.CreatedAt &&
                    x.LastUpdatedAt == category.LastUpdatedAt &&
                    x.TypeEntitie == category.TypeEntitie &&
                    x.Status == category.Status &&
                    x.Id == category.Id
                );
        }

        [Fact]
        public void ValidateUpdate_Message()
        {
            category.Id = Guid.NewGuid();
            category.Name = null;
            categoryRepository.Update(category).Returns(category);

            unitOfWork.ValidateEntity = true;
            categoryService.Invoking(y => y.Update(category))
                .Should().Throw<FluentValidation.ValidationException>();

        }
        
        #endregion

    }
}

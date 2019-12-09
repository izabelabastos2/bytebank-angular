using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System;

namespace Vale.Geographic.Application.Core.Services
{
    public class CategoryAppService : AppService, ICategoryAppService
    {
        private readonly ICategoryRepository CategoryRepository;
        public ICategoryService CategoryService { get; set; }

        public CategoryAppService(IUnitOfWork uoW, IMapper mapper, ICategoryService categoryService,
            ICategoryRepository categoryRepository) : base(uoW, mapper)
        {
            this.CategoryRepository = categoryRepository;
            this.CategoryService = categoryService;
        }

        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                Category category = CategoryService.GetById(id);
                CategoryService.Delete(Mapper.Map<Category>(category));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public CategoryDto GetById(Guid id)
        {
            return Mapper.Map<CategoryDto>(CategoryService.GetById(id));
        }

        public CategoryDto Insert(CategoryDto request)
        {
            try
            {
                UoW.BeginTransaction();
                Category category = Mapper.Map<Category>(request);
                category = CategoryService.Insert(category);
                UoW.Commit();

                return Mapper.Map<CategoryDto>(category);

            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public CategoryDto Update(Guid id, CategoryDto request)
        {
            try
            {
                UoW.BeginTransaction();
                Category category = Mapper.Map<Category>(request);
                category.Id = id;
                category = CategoryService.Update(category);
                UoW.Commit();

                return Mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public IEnumerable<CategoryDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<Category> category = CategoryRepository
             .GetAll(x => true, parameters, new string[] { "Id" })
             .ApplyPagination(parameters, out total)
             .ToList();

            return Mapper.Map<IEnumerable<CategoryDto>>(category);
        }
    }
}

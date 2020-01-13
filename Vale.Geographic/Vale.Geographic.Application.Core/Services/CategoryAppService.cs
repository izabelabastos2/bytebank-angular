using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System;

namespace Vale.Geographic.Application.Core.Services
{
    public class CategoryAppService : AppService, ICategoryAppService
    {
        private readonly ICategoryRepository categoryRepository;
        public ICategoryService categoryService { get; set; }

        public CategoryAppService(IUnitOfWork uoW, IMapper mapper, ICategoryService categoryService,
            ICategoryRepository categoryRepository) : base(uoW, mapper)
        {
            this.categoryRepository = categoryRepository;
            this.categoryService = categoryService;
        }

        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                Category category = categoryService.GetById(id);

                if (category == null)
                    throw new ArgumentNullException();


                categoryService.Delete(Mapper.Map<Category>(category));
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
            return Mapper.Map<CategoryDto>(categoryService.GetById(id));
        }

        public IEnumerable<CategoryDto> Get(Guid? id, bool? active, TypeEntitieEnum? TypeEntitie, IFilterParameters parameters, out int total)
        {
            IEnumerable<Category> categorys = categoryRepository.Get(id, out total, active, TypeEntitie, parameters);

            return Mapper.Map<IEnumerable<CategoryDto>>(categorys);
        }

        public CategoryDto Insert(CategoryDto request)
        {
            try
            {
                UoW.BeginTransaction();
                Category category = Mapper.Map<Category>(request);
                category = categoryService.Insert(category);
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

                var categoryOriginal = categoryRepository.GetById(id);
                UoW.Context.Entry(categoryOriginal).State = EntityState.Detached;

                if (categoryOriginal == null)
                    throw new ArgumentNullException();

                Category category = Mapper.Map<Category>(request);
                category.Id = id;
                category.CreatedAt = categoryOriginal.CreatedAt;

                category = categoryService.Update(category);
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
            IEnumerable<Category> category = categoryRepository
             .GetAll(x => true, parameters, new string[] { "Id" })
             .ApplyPagination(parameters, out total)
             .ToList();

            return Mapper.Map<IEnumerable<CategoryDto>>(category);
        }

    }
}

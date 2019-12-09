
using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface ICategoryAppService
    {
        ICategoryService CategoryService { get; set; }

        void Delete(Guid id);
        CategoryDto GetById(Guid id);
        CategoryDto Insert(CategoryDto obj);
        CategoryDto Update(Guid id, CategoryDto obj);
        IEnumerable<CategoryDto> GetAll(IFilterParameters parameters, out int total);
    }
}

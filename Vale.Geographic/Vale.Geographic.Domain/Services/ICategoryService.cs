﻿using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Services
{
    public interface ICategoryService : IService<Category>
    {
        void InsertAuditory(Category newObj, Category oldObj);
    }
}

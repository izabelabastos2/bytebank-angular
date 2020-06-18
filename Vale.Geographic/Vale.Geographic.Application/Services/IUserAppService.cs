using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Application.Dto.Authorization;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Services
{
    public interface IUserAppService
    {
        IUserService userService { get; set; }

        void Delete(Guid id, string lastUpdatedBy);
        UserDto GetByMatricula(string matricula);
        UserDto GetById(Guid id);
        UserDto Insert(UserDto obj);
        UserDto Update(Guid id, UserDto obj);

    }
}

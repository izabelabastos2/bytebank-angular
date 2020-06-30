using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto.Authorization;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;

namespace Vale.Geographic.Application.Core.Services
{
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IUserRepository userRepository;
        public IUserService userService { get; set; }

        public UserAppService(IUnitOfWork uoW,
                          IMapper mapper,
                          IUserService userService,
                          IUserRepository userRepository) : base(uoW, mapper)
        {
            this.userService = userService;
            this.userRepository = userRepository;
        }

        public void Delete(Guid id, string lastUpdatedBy)
        {
            try
            {
                UoW.BeginTransaction();
                User user = userService.GetById(id);
                User userOriginal = (User)user.Clone();
                UoW.Context.Entry(user).State = EntityState.Detached;

                if (user == null)
                    throw new ArgumentNullException();

                user.Status = false;
                user.LastUpdatedBy = lastUpdatedBy;
                user.LastUpdatedAt = DateTime.UtcNow;

                userService.Update(user);

                userService.InsertAuditory(user, userOriginal);

                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public UserDto GetByMatricula(string matricula)
        {
            return Mapper.Map<UserDto>(userRepository.GetByMatricula(matricula.ToLower()));
        }

        public UserDto GetById(Guid id)
        {
            return Mapper.Map<UserDto>(userService.GetById(id));
        }

        public UserDto Insert(UserDto obj)
        {
            try
            {
                UoW.BeginTransaction();
                User user = Mapper.Map<User>(obj);
                user.LastUpdatedBy = user.CreatedBy;
                user.Matricula = user.Matricula.ToLower();
                //user.Email = user.Email;

                user = userService.Insert(user);
                UoW.Commit();

                return Mapper.Map<UserDto>(user);

            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public UserDto Update(Guid id, UserDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var userOriginal = userRepository.GetById(id);
                UoW.Context.Entry(userOriginal).State = EntityState.Detached;

                if (userOriginal == null)
                    throw new ArgumentNullException();

                User user = Mapper.Map<User>(obj);
                user.Id = id;
                user.CreatedAt = userOriginal.CreatedAt;
                user.CreatedBy = userOriginal.CreatedBy;

                user = userService.Update(user);
                userService.InsertAuditory(user, userOriginal);

                UoW.Commit();

                return Mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}

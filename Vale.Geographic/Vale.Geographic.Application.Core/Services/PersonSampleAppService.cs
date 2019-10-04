﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Vale.Geographic.Application.Base;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Application.Core.Services
{
    public class PersonSampleAppService : AppService, IPersonSampleAppService
    {
        private readonly IPersonSampleRepository _personSampleRepository;

        public PersonSampleAppService(IUnitOfWork uoW, IMapper mapper, IPersonSampleService personSampleService,
            IPersonSampleRepository personSampleRepository) : base(uoW, mapper)
        {
            this.personSampleService = personSampleService;
            this._personSampleRepository = personSampleRepository;
        }

        public IPersonSampleService personSampleService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = personSampleService.GetById(id);
                personSampleService.Delete(Mapper.Map<PersonSample>(obj));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }


        public PersonSampleDto GetById(Guid id)
        {
            return Mapper.Map<PersonSampleDto>(personSampleService.GetById(id));
        }

        public IEnumerable<PersonSampleDto> GetAll(IFilterParameters parameters, out int total)
        {

            var query = _personSampleRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<PersonSampleDto>>(query);

        }

        public IEnumerable<PersonSampleDto> Get(bool? active, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<PersonSampleDto>>(_personSampleRepository.Get(active, request.sort,
                request.page, request.per_page, out total));
        }

        public PersonSampleDto Insert(PersonSampleDto obj)
        {
            try
            {
                UoW.BeginTransaction();
                var PersonSample = personSampleService.Insert(Mapper.Map<PersonSample>(obj));
                UoW.Commit();

                return Mapper.Map<PersonSampleDto>(PersonSample);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public PersonSampleDto Update(Guid id, PersonSampleDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var PersonSample = Mapper.Map<PersonSample>(obj);
                PersonSample.Id = id;
                PersonSample = personSampleService.Update(PersonSample);

                UoW.Commit();
                return Mapper.Map<PersonSampleDto>(PersonSample);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
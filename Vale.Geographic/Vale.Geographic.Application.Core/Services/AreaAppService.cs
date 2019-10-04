using System;
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
    public class AreaAppService : AppService, IAreaAppService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaAppService(IUnitOfWork uoW, IMapper mapper, IAreaService areaService,
            IAreaRepository areaRepository) : base(uoW, mapper)
        {
            this.areaService = areaService;
            this._areaRepository = areaRepository;
        }

        public IAreaService areaService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = areaService.GetById(id);
                areaService.Delete(Mapper.Map<Area>(obj));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }


        public AreaDto GetById(Guid id)
        {
            return Mapper.Map<AreaDto>(areaService.GetById(id));
        }

        public IEnumerable<AreaDto> GetAll(IFilterParameters parameters, out int total)
        {

            var query = _areaRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<AreaDto>>(query);

        }

        public IEnumerable<AreaDto> Get(bool? active, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<AreaDto>>(_areaRepository.Get(active, request.sort,
                request.page, request.per_page, out total));
        }

        public AreaDto Insert(AreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();
                var Area = areaService.Insert(Mapper.Map<Area>(obj));
                UoW.Commit();

                return Mapper.Map<AreaDto>(Area);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public AreaDto Update(Guid id, AreaDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var Area = Mapper.Map<Area>(obj);
                Area.Id = id;
                Area = areaService.Update(Area);

                UoW.Commit();
                return Mapper.Map<AreaDto>(Area);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
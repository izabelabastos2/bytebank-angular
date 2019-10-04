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
    public class SegmentAppService : AppService, ISegmentAppService
    {
        private readonly ISegmentRepository _segmentRepository;

        public SegmentAppService(IUnitOfWork uoW, IMapper mapper, ISegmentService segmentService,
            ISegmentRepository segmentRepository) : base(uoW, mapper)
        {
            this.segmentService = segmentService;
            this._segmentRepository = segmentRepository;
        }

        public ISegmentService segmentService { get; set; }


        public void Delete(Guid id)
        {
            try
            {
                UoW.BeginTransaction();
                var obj = segmentService.GetById(id);
                segmentService.Delete(Mapper.Map<Segment>(obj));
                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }


        public SegmentDto GetById(Guid id)
        {
            return Mapper.Map<SegmentDto>(segmentService.GetById(id));
        }

        public IEnumerable<SegmentDto> GetAll(IFilterParameters parameters, out int total)
        {

            var query = _segmentRepository
                .GetAll(x => true, parameters,
                    new string[] { "FirstName", "LastName", "Type" })
                .ApplyPagination(parameters, out total)
                .ToList();

            return Mapper.Map<IEnumerable<SegmentDto>>(query);

        }

        public IEnumerable<SegmentDto> Get(bool? active, IFilterParameters request, out int total)
        {
            return Mapper.Map<IEnumerable<SegmentDto>>(_segmentRepository.Get(active, request.sort,
                request.page, request.per_page, out total));
        }

        public SegmentDto Insert(SegmentDto obj)
        {
            try
            {
                UoW.BeginTransaction();
                var Segment = segmentService.Insert(Mapper.Map<Segment>(obj));
                UoW.Commit();

                return Mapper.Map<SegmentDto>(Segment);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public SegmentDto Update(Guid id, SegmentDto obj)
        {
            try
            {
                UoW.BeginTransaction();

                var Segment = Mapper.Map<Segment>(obj);
                Segment.Id = id;
                Segment = segmentService.Update(Segment);

                UoW.Commit();
                return Mapper.Map<SegmentDto>(Segment);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class FocalPointAppService : AppService, IFocalPointAppService
    {
        private readonly IFocalPointRepository focalPointRepository;
        private readonly INotificationAnswerRepository notificationAnswerRepository;

        public IFocalPointService focalPointService { get; set; }

        public FocalPointAppService(IUnitOfWork uoW,
                                    IMapper mapper,
                                    IFocalPointService focalPointService,
                                    IFocalPointRepository focalPointRepository,
                                    INotificationAnswerRepository notificationAnswerRepository) : base(uoW, mapper)
        {
            this.focalPointService = focalPointService;
            this.focalPointRepository = focalPointRepository;
            this.notificationAnswerRepository = notificationAnswerRepository;
        }

        public void Delete(Guid id, string lastUpdatedBy)
        {
            try
            {
                UoW.BeginTransaction();
                FocalPoint focalPoint = focalPointService.GetById(id);
                FocalPoint focalPointOriginal = (FocalPoint)focalPoint.Clone();

                UoW.Context.Entry(focalPoint).State = EntityState.Detached;

                if (focalPoint == null)
                    throw new ArgumentNullException();

                focalPoint.Status = false;
                focalPoint.LastUpdatedBy = lastUpdatedBy;
                focalPoint.LastUpdatedAt = DateTime.UtcNow;

                focalPointService.Update(focalPoint);

                focalPointService.InsertAuditory(focalPoint, focalPointOriginal);

                UoW.Commit();
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public FocalPointDto GetById(Guid id)
        {
            return Mapper.Map<FocalPointDto>(focalPointService.GetById(id));
        }
        public FocalPointDto GetByMatricula(string matricula)
        {
            return Mapper.Map<FocalPointDto>(focalPointRepository.GetByMatricula(matricula.ToLower()));
        }

        public FocalPointDto Insert(FocalPointDto request)
        {
            try
            {
                UoW.BeginTransaction();
                FocalPoint focalPoint = Mapper.Map<FocalPoint>(request);
                focalPoint.LastUpdatedBy = focalPoint.CreatedBy;
                focalPoint.Matricula = focalPoint.Matricula.ToLower();
                focalPoint = focalPointService.Insert(focalPoint);
                UoW.Commit();

                return Mapper.Map<FocalPointDto>(focalPoint);

            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public FocalPointDto Update(Guid id, FocalPointDto request)
        {
            try
            {
                UoW.BeginTransaction();

                var focalPointOriginal = focalPointRepository.GetById(id);
                UoW.Context.Entry(focalPointOriginal).State = EntityState.Detached;

                if (focalPointOriginal == null)
                    throw new ArgumentNullException();

                FocalPoint focalPoint = Mapper.Map<FocalPoint>(request);
                focalPoint.Id = id;
                focalPoint.CreatedAt = focalPointOriginal.CreatedAt;
                focalPoint.CreatedBy = focalPointOriginal.CreatedBy;

                focalPoint = focalPointService.Update(focalPoint);
                focalPointService.InsertAuditory(focalPoint, focalPointOriginal);

                UoW.Commit();

                return Mapper.Map<FocalPointDto>(focalPoint);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }
        }

        public IEnumerable<FocalPointDto> GetAll(IFilterParameters parameters, out int total)
        {
            IEnumerable<FocalPoint> focalPoints = focalPointRepository
             .GetAll(x => true, parameters, new string[] { "Id" })
             .ApplyPagination(parameters, out total)
             .ToList();

            return Mapper.Map<IEnumerable<FocalPointDto>>(focalPoints);
        }

        public IEnumerable<FocalPointDto> Get(bool? active, Guid? localityId, Guid? pointOfInterestId, string matricula, IFilterParameters parameters, out int total)
        {
            IEnumerable<FocalPoint> focalPoints = focalPointRepository.Get(out total, active, matricula, localityId, pointOfInterestId, parameters);
            var focalPointsDto = Mapper.Map<IEnumerable<FocalPointDto>>(focalPoints);
            foreach (var focalPoint in focalPointsDto)
            {
                var notificationAnswer = notificationAnswerRepository.GetLastByFocalPointId(focalPoint.Id.Value);
                focalPoint.Answered = notificationAnswer == null ? false: notificationAnswer.Answered;
            }
            return focalPointsDto;
        }

    }
}

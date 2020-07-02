using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Application.Dto.Notification;
using Vale.Geographic.Domain.Entities.Notification;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using Vale.Geographic.Domain.Services;
using Vale.Geographic.Domain.Entities;
using System;
using Vale.Geographic.Application.Dto;

namespace Vale.Geographic.Application.Core.Services
{
    public class NotificationAppService : AppService, INotificationAppService
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationAnswerRepository notificationAnswerRepository;
        private readonly IFocalPointRepository focalPointRepository;
        public INotificationAnswerService notificationAnswerService { get; set; }

        public NotificationAppService(IUnitOfWork uoW, 
                                      IMapper mapper,
                                      INotificationRepository repository,
                                      INotificationAnswerService notificationAnswerService,
                                      INotificationAnswerRepository notificationAnswerRepository,
                                      IFocalPointRepository focalPointRepository) : base(uoW, mapper)
        {
            _repository = repository;
            this.notificationAnswerService = notificationAnswerService;
            this.notificationAnswerRepository = notificationAnswerRepository;
            this.focalPointRepository = focalPointRepository;
        }

        public async Task<DeviceAddDto> RegisterDevice(string applicationId, DeviceAddDto device)
        {
            var ret = await _repository.RegisterDevice(applicationId, Mapper.Map<DeviceAdd>(device));
            return Mapper.Map<DeviceAddDto>(ret);
        }

        public async Task InstalationDevice(string applicationId, string installationId, DeviceUpdateDto deviceUpdate)
        {
            await _repository.InstalationDevice(applicationId, installationId, Mapper.Map<DeviceUpdate>(deviceUpdate));
        }

        public async Task<NotificationAddDto> RegisterNotification(string applicationId, NotificationAddDto notificationAddDto)
        {

            var notification = Mapper.Map<NotificationAdd>(notificationAddDto);
            notification.NotId = Guid.NewGuid();
            var ret = await _repository.RegisterNotification(applicationId, notification);
            string matricula = notificationAddDto.Categories.Select(o => o.Tag).ToArray().FirstOrDefault();

            SaveNotificationAnswer(notification.NotId, matricula, ret.CreatedBy);
            return Mapper.Map<NotificationAddDto>(ret);                
            
        }

        private NotificationAnswerDto SaveNotificationAnswer(Guid notificationId, string matricula, string createdBy)
        {
            try
            {
                UoW.BeginTransaction();                
                NotificationAnswerDto notificationAnswer = new NotificationAnswerDto();
                FocalPoint focalPoint = focalPointRepository.GetByMatricula(matricula.ToLower());
                notificationAnswer.FocalPointId = focalPoint.Id;
                notificationAnswer.NotificationId = notificationId;
                notificationAnswer.CreatedBy = createdBy;
                notificationAnswer.LastUpdatedBy = createdBy;

                var ret =  notificationAnswerService.Insert(Mapper.Map<NotificationAnswer>(notificationAnswer));

                UoW.Commit();

                return Mapper.Map<NotificationAnswerDto>(ret);
            }
            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }

        }

        public NotificationAnswerDto UpdateNotificationAnswer(string applicationId, Guid notificationId)
        {
            try
            {
                UoW.BeginTransaction();
                var notificationAnswer = notificationAnswerService.GetAll().Where(o => o.NotificationId == notificationId).SingleOrDefault();

                if (notificationAnswer == null)
                    throw new ArgumentNullException();

                notificationAnswerService.Update(notificationAnswer);
                UoW.Commit();

                return Mapper.Map<NotificationAnswerDto>(notificationAnswer);
            }

            catch (Exception ex)
            {
                UoW.Rollback();
                throw ex;
            }

            }

        public NotificationAnswerDto GetLastNotification(string applicationId, Guid focalPointId)
        {
            return Mapper.Map<NotificationAnswerDto>(notificationAnswerRepository.GetLastByFocalPointId(focalPointId));
        }
    }
}

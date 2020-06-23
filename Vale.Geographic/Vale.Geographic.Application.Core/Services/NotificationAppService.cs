using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Application.Dto.Notification;
using Vale.Geographic.Domain.Entities.Notification;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Application.Services;
using Vale.Geographic.Application.Base;
using System.Threading.Tasks;
using AutoMapper;

namespace Vale.Geographic.Application.Core.Services
{
    public class NotificationAppService : AppService, INotificationAppService
    {
        private readonly INotificationRepository _repository;

        public NotificationAppService(IUnitOfWork uoW, IMapper mapper, INotificationRepository repository) : base(uoW, mapper)
        {
            _repository = repository;
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
            var ret = await _repository.RegisterNotification(applicationId, Mapper.Map<NotificationAdd>(notificationAddDto));
            return Mapper.Map<NotificationAddDto>(ret);
        }
    }
}

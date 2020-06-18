using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Dto.Notification;
using System.Threading.Tasks;

namespace Vale.Geographic.Application.Services
{
    public interface INotificationAppService
    {
        Task InstalationDevice(string applicationId, string installationId, DeviceUpdateDto deviceUpdate);
        Task<DeviceAddDto> RegisterDevice(string applicationId, DeviceAddDto device);
        Task<NotificationAddDto> RegisterNotification(string applicationId, NotificationAddDto notificationAddDto);
    }
}

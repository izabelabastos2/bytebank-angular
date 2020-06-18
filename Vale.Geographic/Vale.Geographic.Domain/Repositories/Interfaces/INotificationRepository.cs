using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vale.Geographic.Domain.Entities.Notification;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task InstalationDevice(string applicationId, string installationId, DeviceUpdate deviceUpdate);
        Task<DeviceAdd> RegisterDevice(string applicationId, DeviceAdd device);
        Task<NotificationAdd> RegisterNotification(string applicationId, NotificationAdd notificationAddDto);
    }
}

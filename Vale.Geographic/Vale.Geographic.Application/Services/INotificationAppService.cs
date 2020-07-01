using System;
using System.Collections.Generic;
using Vale.Geographic.Application.Dto.Notification;
using System.Threading.Tasks;
using Vale.Geographic.Application.Dto;
using Vale.Geographic.Domain.Services;


namespace Vale.Geographic.Application.Services
{
    public interface INotificationAppService
    {
        INotificationAnswerService notificationAnswerService { get; set; }

        Task InstalationDevice(string applicationId, string installationId, DeviceUpdateDto deviceUpdate);
        Task<DeviceAddDto> RegisterDevice(string applicationId, DeviceAddDto device);
        Task<NotificationAddDto> RegisterNotification(string applicationId, NotificationAddDto notificationAddDto);
        NotificationAnswerDto UpdateNotificationAnswer(string applicationId, Guid notification);
        NotificationAnswerDto GetLastNotification(string applicationId, string focalPointId);
    }
}

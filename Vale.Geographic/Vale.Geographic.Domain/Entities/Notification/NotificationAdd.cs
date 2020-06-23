using System;
using System.Collections.Generic;
using System.Text;

namespace Vale.Geographic.Domain.Entities.Notification
{
    public class NotificationAdd
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Image { get; set; }

        public string AzureNotificationId { get; set; }

        public string Sound { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public CategoryNotification[] Categories { get; set; }
    }
}

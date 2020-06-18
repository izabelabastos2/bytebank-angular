using System;

namespace Vale.Geographic.Domain.Entities.Notification
{
    public class DeviceAdd
    {
        public long Id { get; set; }
        public string IamId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Firmware { get; set; }
        public string Hardware { get; set; }
        public string Region { get; set; }
        public string Manufacturer { get; set; }
        public string Culture { get; set; }
        public DateTimeOffset DeviceTime { get; set; }
        public string Platform { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

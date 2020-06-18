namespace Vale.Geographic.Domain.Entities.Notification
{
    public class DeviceUpdate
    {
        public string Platform { get; set; }

        public string Handle { get; set; }

        public string[] Tags { get; set; }
    }
}

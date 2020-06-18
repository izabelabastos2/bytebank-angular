using System;
using System.Collections.Generic;
using System.Text;

namespace Vale.Geographic.Application.Dto.Notification
{
    public class DeviceUpdateDto
    {
        public string Platform { get; set; }

        public string Handle { get; set; }

        public string[] Tags { get; set; }
    }
}

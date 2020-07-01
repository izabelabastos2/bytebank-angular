using System;
using System.Collections.Generic;
using System.Text;

namespace Vale.Geographic.Application.Dto.Notification
{
    public class ActionAddDto
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Callback { get; set; }

        public bool Foreground { get; set; }
    }
}

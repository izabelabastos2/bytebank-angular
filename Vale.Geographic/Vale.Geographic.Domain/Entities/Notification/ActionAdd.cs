using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vale.Geographic.Domain.Entities.Notification
{
    public class ActionAdd
    {        
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Callback { get; set; }

        public bool Foreground { get; set; }

    }
}


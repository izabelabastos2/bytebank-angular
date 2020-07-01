using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Base;

namespace Vale.Geographic.Domain.Entities
{
    public class NotificationAnswer : Entity
    {
        public Guid FocalPointId { get; set; }        
        public Guid NotificationId { get; set; }
        public bool Answered { get; set; }
    }
}

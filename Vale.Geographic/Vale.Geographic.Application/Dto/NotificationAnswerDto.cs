using System;
using System.Collections.Generic;
using System.Text;

namespace Vale.Geographic.Application.Dto
{
    public class NotificationAnswerDto
    {
        public Guid? Id { get; set; }
        public Guid FocalPointId { get; set; }        
        public long NotificationId { get; set; }
        public bool Answered { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool Status { get; set; }

    }
}


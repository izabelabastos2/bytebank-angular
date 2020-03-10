using Vale.Geographic.Domain.Enumerable;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class AuditoryDto
    {
        public Guid? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool Status { get; set; }

        public TypeEntitieEnum TypeEntitie { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? PointOfInterestId { get; set; }

        public Guid? CategoryId { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual PointOfInterestDto PointOfInterest { get; set; }

        public virtual CategoryDto Category { get; set; }
    }
}

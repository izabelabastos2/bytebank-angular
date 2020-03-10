using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Auditory : Entity
    {
        [Required]
        public TypeEntitieEnum TypeEntitie { get; set; }
        [Required]
        public string OldValue { get; set; }
        [Required]
        public string NewValue { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? PointOfInterestId { get; set; }

        public Guid? CategoryId { get; set; }

        [Write(false)]
        public virtual Area Area { get; set; }

        [Write(false)]
        public virtual PointOfInterest PointOfInterest { get; set; }

        [Write(false)]
        public virtual Category Category { get; set; }

        public Auditory() {}

    }
}

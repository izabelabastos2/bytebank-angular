
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;

namespace Vale.Geographic.Domain.Entities
{
    public class PointOfInterest : Entity
    {
        public PointOfInterest() { }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public virtual Point Location { get; set; }
    }
}

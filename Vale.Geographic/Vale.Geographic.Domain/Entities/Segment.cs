
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;

namespace Vale.Geographic.Domain.Entities
{
    public class Segment : Entity
    {
        public Segment() { } 

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public virtual Geometry Location { get; set; }
        
        [Required]
        public virtual Guid RouteId { get; set; }
    }
}

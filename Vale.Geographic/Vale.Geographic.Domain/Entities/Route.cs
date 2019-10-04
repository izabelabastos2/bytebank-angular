
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;

namespace Vale.Geographic.Domain.Entities
{
    public class Route : Entity
    {
        public Route() { }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public virtual Geometry Location { get; set; }

        public virtual List<Segment> Segments { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using System;

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
        public virtual IGeometry Location { get; set; }

        [Required]
        public virtual Guid RouteId { get; set; }

        [Write(false)]
        public Route Route { get; set; }

        public Guid AreaId { get; set; }

        [Write(false)]
        public Area Area { get; set; }
    }
}

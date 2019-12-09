
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;

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
        public virtual IGeometry Location { get; set; }

        //public virtual List<Segment> Segments { get; set; }

        public Guid AreaId { get; set; }

        [Write(false)]
        public Area Area { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vale.Geographic.Domain.Entities
{
    public class Area : Entity
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual Geometry Location { get; set; }
        
        public virtual Area Parent{ get; set; }

        public virtual List<Area> SubAreas { get; set; }

        public virtual List<PointOfInterest> PointsOfInterest { get; set; }

        public virtual List<Route> Routes { get; set; }

        public virtual List<Segment> Segments { get; set; }

    }
}

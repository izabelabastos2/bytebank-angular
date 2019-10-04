using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Vale.Geographic.Application.Dto
{
    public class AreaDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual IGeometry Location { get; set; }

        public virtual AreaDto Parent { get; set; }

        public virtual List<AreaDto> SubAreas { get; set; }

        public virtual List<PointOfInterestDto> PointsOfInterest { get; set; }

        public virtual List<RouteDto> Routes { get; set; }

        public virtual List<SegmentDto> Segments { get; set; }
    }
}



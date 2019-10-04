using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Vale.Geographic.Application.Dto
{
    public class SegmentDto
    {
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
    }
}

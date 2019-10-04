using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Vale.Geographic.Application.Dto
{
    public class PointOfInterestDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual IPoint Location { get; set; }
    }
}



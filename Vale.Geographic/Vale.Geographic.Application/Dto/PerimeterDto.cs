using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;
using GeoJSON.Net.Geometry;
using System.Collections.Generic;

namespace Vale.Geographic.Application.Dto
{
    public class PerimeterDto
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Name { get; set; }

        [Required]
        public bool Status { get; set; }

        public Feature Geojson { get; set; }

        public List<Guid> Sites { get; set; }
    }
}



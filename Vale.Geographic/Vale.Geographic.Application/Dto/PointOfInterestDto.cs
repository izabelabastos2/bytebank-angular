using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class PointOfInterestDto
    {
        public Guid? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Feature Geojson { get; set; }

        [Required]
        public Guid AreaId { get; set; }

        public AreaDto Area { get; set; }

        public Guid? CategoryId { get; set; }

        public CategoryDto Category { get; set; }
    }
}



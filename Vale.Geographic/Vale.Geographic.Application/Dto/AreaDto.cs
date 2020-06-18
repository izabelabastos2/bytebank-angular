using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;
using GeoJSON.Net.Geometry;

namespace Vale.Geographic.Application.Dto
{
    public class AreaDto
    {
        public Guid? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        [Required]
        public Feature Geojson { get; set; }

        public Feature CentralPoint { get; set; }

        public Guid? CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public Guid? ParentId { get; set; }

        public AreaDto Parent { get; set; }

    }
}



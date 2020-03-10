using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class AreaDtoTest
    {
        public Guid? Id { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        [Required]
        public Feature Geojson { get; set; }

        public Guid? CategoryId { get; set; }

    }
}

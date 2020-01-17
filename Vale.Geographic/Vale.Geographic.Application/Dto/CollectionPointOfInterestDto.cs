using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class CollectionPointOfInterestDto
    {
        [Required]
        public FeatureCollection Geojson { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? CategoryId { get; set; }
    }
}

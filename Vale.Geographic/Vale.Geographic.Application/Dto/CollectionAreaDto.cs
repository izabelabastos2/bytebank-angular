using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Feature;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class CollectionAreaDto
    {
       [Required]
        public FeatureCollection Geojson { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? ParentId { get; set; }

    }
}

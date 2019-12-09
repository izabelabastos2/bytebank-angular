using GeoAPI.Geometries;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vale.Geographic.Application.Dto
{
    public class AreaDto
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public bool Status { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

      //  [Required]
        //public virtual IGeometry Location { get; set; }

       // [Required]
       // public virtual Feature GeoJson { get; set; }

        public virtual MultiPolygon Location { get; set; }

        public Guid? CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public virtual AreaDto Parent { get; set; }
    }
}



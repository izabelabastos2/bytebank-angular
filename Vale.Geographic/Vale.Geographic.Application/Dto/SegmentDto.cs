using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Geometry;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class SegmentDto
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public bool Status { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public virtual MultiPolygon Location { get; set; }

        //public virtual IGeometry Location { get; set; }

        [Required]
        public virtual Guid RouteId { get; set; }

        public RouteDto Route { get; set; }

        public Guid AreaId { get; set; }

        public AreaDto Area { get; set; }

    }
}

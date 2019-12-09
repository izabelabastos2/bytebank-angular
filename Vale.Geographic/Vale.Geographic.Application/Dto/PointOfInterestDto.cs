using System.ComponentModel.DataAnnotations;
using GeoJSON.Net.Geometry;
using System;


namespace Vale.Geographic.Application.Dto
{
    public class PointOfInterestDto
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public bool Status { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Point Location { get; set; }

        //[Required]
        //public virtual IPoint Location { get; set; }

        public Guid AreaId { get; set; }

        public AreaDto Area { get; set; }

        public Guid? CategoryId { get; set; }

        public CategoryDto Category { get; set; }
    }
}



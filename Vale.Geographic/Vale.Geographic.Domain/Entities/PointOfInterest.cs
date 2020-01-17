using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class PointOfInterest : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        [Required]
        public virtual IGeometry Location { get; set; }

        public virtual Guid? AreaId { get; set; }

        [Write(false)]
        public virtual Area Area { get; set; }

        public virtual Guid? CategoryId { get; set; }

        [Write(false)]
        public virtual Category Category { get; set; }

        public PointOfInterest() { }

    }
}

using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Area : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual IGeometry Location { get; set; }   

        public virtual Guid? CategoryId { get; set; }

        [Write(false)]
        public virtual Category Category { get; set; }

        public virtual Guid? ParentId { get; set; }

        [Write(false)]
        public virtual Area Parent{ get; set; }

        public Area() { }
    }
}

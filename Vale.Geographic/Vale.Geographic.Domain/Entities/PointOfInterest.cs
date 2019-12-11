
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using NetTopologySuite.Geometries;
using Dapper.Contrib.Extensions;


namespace Vale.Geographic.Domain.Entities
{
    public class PointOfInterest : Entity
    {
        public PointOfInterest() { }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public virtual Geometry Location { get; set; }
        //public virtual Point Location { get; set; }

        [Required]
        public Guid AreaId { get; set; }

        [Write(false)]
        public Area Area { get; set; }

        public Guid? CategoryId { get; set; }

        [Write(false)]
        public Category Category { get; set; }
    }
}

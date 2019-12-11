using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using System;
using NetTopologySuite.Geometries;

namespace Vale.Geographic.Domain.Entities
{
    public class Area : Entity
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual Geometry Location { get; set; }   

        public Guid? CategoryId { get; set; }

        [Write(false)]
        public Category Category { get; set; }

        [Write(false)]
        public virtual Area Parent{ get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Site : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        [Required]
        public int Zoom { get; set; }

        public int Radius { get; set; }

        public Guid? ParentId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Site Parent { get; set; }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

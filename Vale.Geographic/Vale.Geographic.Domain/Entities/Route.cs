using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Route : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public virtual IGeometry Location { get; set; }

        [Required]
        [JsonIgnore]
        public virtual Guid AreaId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Area Area { get; set; }

        public Route() { }

        public bool Equals(Route route)
        {
            if ((object)route == null)
                return false;

            return (Id == route.Id) && (Name == route.Name) && (CreatedAt == route.CreatedAt)
                && (Status == route.Status) && (Description == route.Description)
                && (Length == route.Length) && (Location.EqualsExact(route.Location))
                && (AreaId == route.AreaId);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

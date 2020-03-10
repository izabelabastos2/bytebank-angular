using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Segment : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public virtual IGeometry Location { get; set; }

        [Required]
        public virtual Guid RouteId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Route Route { get; set; }

        [Required]
        public virtual Guid AreaId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Area Area { get; set; }

        public Segment() { }

        public bool Equals(Segment segment)
        {
            if ((object)segment == null)
                return false;

            return (Id == segment.Id) && (Name == segment.Name) && (CreatedAt == segment.CreatedAt)
                && (Status == segment.Status) && (Description == segment.Description)
                && (Length == segment.Length) && (Location.EqualsExact(segment.Location))
                && (AreaId == segment.AreaId) && (RouteId == segment.RouteId);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

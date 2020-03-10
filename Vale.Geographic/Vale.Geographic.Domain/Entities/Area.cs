using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Area : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual IGeometry Location { get; set; }   

        public virtual Guid? CategoryId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public virtual Guid? ParentId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Area Parent{ get; set; }

        public Area() { }

        public bool Equals(Area area)
        {
            if ((object)area == null)
                return false;

            return (Id == area.Id) && (Name == area.Name) && (CreatedAt == area.CreatedAt)
                && (Status == area.Status) && (Description == area.Description)
                && (Color == area.Color) && (Location.EqualsExact(area.Location))
                && (ParentId == area.ParentId) && (CategoryId == area.CategoryId);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class PointOfInterest : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        [Required]
        public virtual IGeometry Location { get; set; }

        public virtual Guid? AreaId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Area Area { get; set; }

        public virtual Guid? CategoryId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public PointOfInterest() { }

        public bool Equals(PointOfInterest point)
        {
            if ((object)point == null)
                return false;

            return (Id == point.Id) && (Name == point.Name) && (CreatedAt == point.CreatedAt)
                && (Status == point.Status) && (Description == point.Description) 
                && (Icon == point.Icon) && (Location.EqualsExact(point.Location)) 
                && (AreaId == point.AreaId) && (CategoryId == point.CategoryId);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

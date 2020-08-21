using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class SitesPerimeters : Entity, ICloneable
    {
        [Required]
        public Guid SiteId { get; set; }

        [Required]
        public Guid AreaId { get; set; }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

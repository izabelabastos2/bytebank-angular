using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;
using Dapper.Contrib.Extensions;
using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class SitesPerimeter : Entity, ICloneable
    {
        [Required]
        public virtual Guid SiteId { get; set; }

        [Required]
        public virtual Guid AreaId { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Area Area { get; set; }

        [Write(false)]
        [JsonIgnore]
        public virtual Site Site { get; set; }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vale.Geographic.Application.Dto
{
    public class GeoJsonDto
    {
       [Required]
        public virtual FeatureCollection FeatureCollection { get; set; }

        public Guid? CategoryId { get; set; }
    }
}

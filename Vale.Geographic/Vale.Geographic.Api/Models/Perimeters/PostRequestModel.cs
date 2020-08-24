using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vale.Geographic.Api.Models.Perimeters
{
    public class PostRequestModel
    {
        public string Name { get; set; }

        public bool Status { get; set; }

        public Feature Geojson { get; set; }

        public List<Guid> Sites { get; set; }
    }
}

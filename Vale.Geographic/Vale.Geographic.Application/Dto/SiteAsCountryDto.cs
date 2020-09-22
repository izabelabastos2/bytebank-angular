using System;
using System.Collections.Generic;

namespace Vale.Geographic.Application.Dto
{
    public class SiteAsCountryDto : SiteDto
    {
        public IEnumerable<SiteAsStateDto> States;
    }

    public class SiteAsStateDto : SiteDto
    {
        public IEnumerable<SiteDto> Units;
    }

    public class SiteDto
    {
        public string Name { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public int? Zoom { get; set; }
        public int Radius { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}

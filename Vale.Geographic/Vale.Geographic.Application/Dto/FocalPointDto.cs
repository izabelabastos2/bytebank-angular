using System.ComponentModel.DataAnnotations;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class FocalPointDto
    {
        public Guid? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool Status { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Matricula { get; set; }

        public string PhoneNumber { get; set; }

        public bool Answered { get; set; }

        [Required]
        public Guid LocalityId { get; set; }

        [Required]
        public Guid PointOfInterestId { get; set; }

        public AreaDto Locality { get; set; }

        public PointOfInterestDto PointOfInterest { get; set; }

    }
}

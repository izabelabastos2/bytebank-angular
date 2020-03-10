using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using System;

namespace Vale.Geographic.Application.Dto
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public TypeEntitieEnum TypeEntitie { get; set; }

        [Required] 
        public string Name { get; set; }
    }
}

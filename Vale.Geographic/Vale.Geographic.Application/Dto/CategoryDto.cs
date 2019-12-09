using System;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Application.Dto
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public bool Status { get; set; }

        [Required]
        public TypeEntitieEnum TypeEntitie { get; set; }

        [Required] 
        public string Description { get; set; }
    }
}

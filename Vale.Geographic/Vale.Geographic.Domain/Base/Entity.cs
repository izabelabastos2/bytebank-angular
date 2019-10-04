using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vale.Geographic.Domain.Base
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using System;

namespace Vale.Geographic.Domain.Base
{
    public class Entity
    {
        [ExplicitKey]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Status { get; set; }
    }
}
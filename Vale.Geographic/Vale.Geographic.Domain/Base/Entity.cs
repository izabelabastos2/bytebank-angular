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
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
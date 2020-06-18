using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using System;

namespace Vale.Geographic.Application.Dto.Authorization
{
    public class UserDto
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

        public string Email { get; set; }

        [Required]
        public RoleEnum Profile { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Base;

namespace Vale.Geographic.Domain.Entities.Authorization
{
    public class User : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Matricula { get; set; }
        public string Email { get; set; }
        [Required]
        public RoleEnum Profile { get; set; }

        public bool Equals(User user)
        {
            if ((object)user == null)
                return false;

            return (Id == user.Id) && (Name.Equals(user.Name)) && (CreatedAt == user.CreatedAt)
                && (Status == user.Status) && (Profile == user.Profile) && (Matricula.Equals(user.Matricula))
                 && (Email.Equals(user.Email) );
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

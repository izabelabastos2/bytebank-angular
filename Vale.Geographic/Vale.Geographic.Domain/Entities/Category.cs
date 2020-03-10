using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Base;
using System;

namespace Vale.Geographic.Domain.Entities
{
    public class Category : Entity, ICloneable
    {
        [Required]
        public TypeEntitieEnum TypeEntitie { get; set; }

        [Required]
        public string Name { get; set; }

        public Category(){ }

        public bool Equals(Category category)
        {
            if ((object)category == null)
                return false;

            return (Id == category.Id) && (Name == category.Name) && (CreatedAt == category.CreatedAt)
                && (Status == category.Status) && (TypeEntitie == category.TypeEntitie) ;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

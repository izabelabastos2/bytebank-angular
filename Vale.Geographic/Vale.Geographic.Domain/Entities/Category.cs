using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Base;

namespace Vale.Geographic.Domain.Entities
{
    public class Category : Entity
    {
        [Required]
        public TypeEntitieEnum TypeEntitie { get; set; }

        [Required]
        public string Name { get; set; }

        public Category(){ }
    }
}

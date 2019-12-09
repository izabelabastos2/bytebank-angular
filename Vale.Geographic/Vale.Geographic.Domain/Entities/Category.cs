using Vale.Geographic.Domain.Enumerable;
using Vale.Geographic.Domain.Base;

namespace Vale.Geographic.Domain.Entities
{
    public class Category : Entity
    {
        public TypeEntitieEnum TypeEntitie { get; set; }

        public string Description { get; set; }

        public Category(){ }
    }
}

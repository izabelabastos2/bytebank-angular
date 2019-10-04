using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vale.Geographic.Domain.Base;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Domain.Entities
{
    public class PersonSample : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public PersonTypeSampleEnum Type { get; set; }
        public bool Active { get; set; }
        public int Age { get; set; }
    }
}
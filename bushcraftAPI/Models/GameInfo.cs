using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bushcraftAPI.Models
{
    public class GameInfo : Resource
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }
        public ICollection<Person> Designers { get; set; } 
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
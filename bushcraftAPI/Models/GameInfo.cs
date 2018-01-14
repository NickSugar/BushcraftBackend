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
        public string DesignerName { get; set; }
        public ICollection<Designer> Designers { get; set; } 
    }

    public class Designer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

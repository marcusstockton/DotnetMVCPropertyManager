using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class Portfolio : Base
    {
        public string Name { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual IList<Property> Properties { get; set; }
    }
}

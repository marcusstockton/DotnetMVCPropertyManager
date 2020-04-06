using System.Collections.Generic;

namespace Website.Models
{
    public class Portfolio : Base
    {
        public string Name { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual IList<Property> Properties { get; set; }
    }
}
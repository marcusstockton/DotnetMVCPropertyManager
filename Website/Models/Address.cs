using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class Address : Base
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public virtual Property Property { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.DTOs
{
    public class AddressDTO
    {
        [Display(Name = "Line 1")]
        public string Line1 { get; set; }

        [Display(Name = "Line 2")]
        public string Line2 { get; set; }

        [Display(Name = "Line 2")]
        public string Line3 { get; set; }
        [Display(Name = "Post Code"), Required]
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs
{
    public class AddressDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [Display(Name = "Line 1")]
        public string Line1 { get; set; }

        [Display(Name = "Line 2")]
        public string Line2 { get; set; }

        [Display(Name = "Line 3")]
        public string Line3 { get; set; }

        [Display(Name = "Post Code"), Required, DataType(DataType.PostalCode)]
        public string Postcode { get; set; }

        public string Town { get; set; }
        public string City { get; set; }
    }
}
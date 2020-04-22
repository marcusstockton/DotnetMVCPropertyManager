using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Tenant : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        public DateTime TenancyStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TenancyEndDate { get; set; }
        public virtual Property Property { get; set; }
        public string TenantImage { get; set; }
        public virtual List<Note> Notes { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Website.Models
{
    public class Tenant : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public string Nationality { get; set; }
        public DateTime TenancyStartDate { get; set; }
        public DateTime? TenancyEndDate { get; set; }
        public virtual Property Property { get; set; }
        public string TenantImage { get; set; }
        public virtual List<Note> Notes { get; set; }
    }
}
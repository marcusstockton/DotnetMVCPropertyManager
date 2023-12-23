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

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string JobTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset TenancyStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? TenancyEndDate { get; set; }

        public virtual Property Property { get; set; }
        public string TenantImage { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual Nationality? Nationality { get; set; }
    }
}
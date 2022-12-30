using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Tenants
{
    public class TenantDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTimeOffset UpdatedDate { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Date Tenancy Started"), DataType(DataType.Date)]
        public DateTimeOffset TenancyStartDate { get; set; }

        [Display(Name = "Date Tenancy Ended"), DataType(DataType.Date)]
        public DateTimeOffset? TenancyEndDate { get; set; }

        [Display(Name = "Tenant Profile")]
        public string TenantImage { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual Nationality Nationality { get; set; }
        public virtual Property Property { get; set; }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Tenants
{
    public class TenantCreateDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        public string Nationality { get; set; }

        [Display(Name = "Date Tenancy Started")]
        [DataType(DataType.Date)]
        public DateTimeOffset TenancyStartDate { get; set; }

        [Display(Name = "Date Tenancy Ended")]
        [DataType(DataType.Date)]
        public DateTimeOffset? TenancyEndDate { get; set; }

        public virtual Property Property { get; set; }

        [Display(Name = "Tenant Profile"), DataType(DataType.Upload)]
        public IFormFile TenantImage { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}
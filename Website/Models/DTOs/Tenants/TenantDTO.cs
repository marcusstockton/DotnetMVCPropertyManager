﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Tenants
{
    public class TenantDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        public string Nationality { get; set; }

        [Display(Name = "Date Tenancy Started"), DataType(DataType.Date)]
        public DateTime TenancyStartDate { get; set; }

        [Display(Name = "Date Tenancy Ended"), DataType(DataType.Date)]
        public DateTime? TenancyEndDate { get; set; }

        public virtual Property Property { get; set; }

        [Display(Name = "Tenant Profile")]
        public string TenantImage { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}
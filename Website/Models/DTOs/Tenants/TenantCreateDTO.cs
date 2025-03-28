﻿using Microsoft.AspNetCore.Http;
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

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAdresss { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Nationality")]
        public int NationalityId { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTimeOffset? DateOfBirth { get; set; }

        [Display(Name = "Date Tenancy Started")]
        [DataType(DataType.Date)]
        public DateTimeOffset TenancyStartDate { get; set; }

        [Display(Name = "Date Tenancy Ended")]
        [DataType(DataType.Date)]
        public DateTimeOffset? TenancyEndDate { get; set; }

        [Display(Name = "Smoker?")]
        public bool IsSmoker { get; set; }

        [Display(Name = "Has Pets?")]
        public bool HasPets { get; set; }

        public virtual Property Property { get; set; }

        [Display(Name = "Tenant Profile"), DataType(DataType.Upload)]
        public IFormFile TenantImage { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}
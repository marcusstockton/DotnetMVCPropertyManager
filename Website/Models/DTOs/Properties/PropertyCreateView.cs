using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Website.Models.DTOs.Documents;

namespace Website.Models.DTOs.Properties
{
    [Serializable]
    public class PropertyCreateView
    {
        public PropertyCreateView()
        {
            Documents = new List<DocumentUploader>();
        }

        [Display(Name = "Date Of Purchase"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}"), DataType(DataType.Date)]
        public DateTimeOffset? PurchaseDate { get; set; }

        [Display(Name = "Property Value")]
        public double PropertyValue { get; set; }

        [Display(Name = "# Rooms"), Range(1, 100, ErrorMessage = "You cannot have a property with more than 100 rooms.")]
        public int NoOfRooms { get; set; }

        [Display(Name = "Monthly Rental Amount"), Range(1, 10000, ErrorMessage = "You cannot charge more than 10000 per month.")]
        public double MonthlyRentAmount { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public virtual AddressDTO Address { get; set; }
        public virtual List<Tenant> Tenants { get; set; }
        public List<DocumentUploader> Documents { get; set; }
        public virtual List<IFormFile> Images { get; set; }
    }
}
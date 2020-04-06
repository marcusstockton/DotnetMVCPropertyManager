using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Website.Models.DTOs.Documents;

namespace Website.Models.DTOs
{
    public class PropertyCreateView
    {
        public PropertyCreateView()
        {
            Documents = new List<DocumentUploader>();
        }

        [Display(Name = "Date Of Purchase"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}"), DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Property Value")]
        public double PropertyValue { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public virtual AddressDTO Address { get; set; }
        public virtual List<Tenant> Tenants { get; set; }
        public virtual List<DocumentUploader> Documents { get; set; }
        public virtual List<IFormFile> Images { get; set; }
    }
}
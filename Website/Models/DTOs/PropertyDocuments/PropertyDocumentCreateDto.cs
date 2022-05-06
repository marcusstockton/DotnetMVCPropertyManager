using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.PropertyDocuments
{
    public class PropertyDocumentCreateDto
    {
        [DataType(DataType.Upload)]
        public IFormFile Document { get; set; }

        [Display(Name = "Document Type")]
        public Guid DocumentTypeId { get; set; }

        public bool Expires { get; set; }

        [Display(Name = "Active From")]
        public DateTimeOffset? ActiveFrom { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTimeOffset? ExpiryDate { get; set; }

        public Guid PropertyId { get; set; }
    }
}
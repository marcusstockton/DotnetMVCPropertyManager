using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Documents
{
    public class DocumentUploader
    {
        [DataType(DataType.Upload)]
        public IFormFile Document { get; set; }

        [Display(Name = "Document Type")]
        public DocumentType DocumentType { get; set; }

        [DataType(DataType.Date), Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }
    }
}
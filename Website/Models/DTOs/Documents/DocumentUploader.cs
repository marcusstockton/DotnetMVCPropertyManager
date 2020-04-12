using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Documents
{
    public class DocumentUploader
    {
        [DataType(DataType.Upload), BindProperty, FileExtensions(Extensions = "doc,docx,xml,xlsx,rtf,pdf,txt")]
        public IFormFile Document { get; set; }

        [Display(Name = "Document Type")]
        public virtual DocumentType DocumentType { get; set; }

        [DataType(DataType.Date), Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }
    }
}
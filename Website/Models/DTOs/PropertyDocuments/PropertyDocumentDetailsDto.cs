using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.PropertyDocuments
{
    public class PropertyDocumentDetailsDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        [Display(Name = "File Type")]
        public string FileType { get; set; }
        public Guid PropertyId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; } // EPC, Certificates etc...
        public virtual Property Property { get; set; }
        [Display(Name="Expiration Date"), DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; } // When the document runs out.
        public bool Expires { get; set; }
        [Display(Name = "Active From"), DataType(DataType.Date)]
        public DateTime? ActiveFrom { get; set; }
    }
}
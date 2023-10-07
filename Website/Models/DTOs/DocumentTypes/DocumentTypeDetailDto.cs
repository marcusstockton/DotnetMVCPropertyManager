using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.DocumentTypes
{
    public class DocumentTypeDetailDto
    {
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Updated Date")]
        public DateTimeOffset? UpdatedDate { get; set; }

        public string Description { get; set; }
        public bool Expires { get; set; }

        [DataType(DataType.Date), Display(Name = "Expiry Date")]
        public DateTimeOffset? ExpiryDate { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}
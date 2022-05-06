using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.DocumentTypes
{
    public class DocumentTypeDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }

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
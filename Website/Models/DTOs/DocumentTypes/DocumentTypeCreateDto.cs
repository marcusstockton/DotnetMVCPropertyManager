using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.DTOs.DocumentTypes
{
    public class DocumentTypeCreateDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Description { get; set; }
        public bool Expires { get; set; } // Documents that use this type, have an expiration
        public System.DateTime ExpriyDate { get; set; }
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}

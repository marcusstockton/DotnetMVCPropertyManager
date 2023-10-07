using System;

namespace Website.Models
{
    public class DocumentType : Base
    {
        public string Description { get; set; }
        public bool Expires { get; set; } // Documents that use this type, have an expiration
        public DateTimeOffset ExpiryDate { get; set; }
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}
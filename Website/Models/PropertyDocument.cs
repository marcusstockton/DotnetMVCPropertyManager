using System;

namespace Website.Models
{
    public class PropertyDocument : Base
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public Guid PropertyId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; } // EPC, Certificates etc...
        public virtual Property Property { get; set; }
        public bool Expires { get; set; }
        public DateTimeOffset? ActiveFrom { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; } // When the document runs out.
    }
}
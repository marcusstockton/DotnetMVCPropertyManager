namespace Website.Models
{
    public class DocumentType : Base
    {
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}
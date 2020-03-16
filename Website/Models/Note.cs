namespace Website.Models
{
    public class Note : Base
    {
        public virtual Tenant Tenant { get; set; }
        public string Description { get; set; }
    }
}
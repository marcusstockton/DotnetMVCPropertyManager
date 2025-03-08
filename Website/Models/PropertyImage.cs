namespace Website.Models
{
    public class PropertyImage : Base
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public virtual Property Property { get; set; }
    }
}
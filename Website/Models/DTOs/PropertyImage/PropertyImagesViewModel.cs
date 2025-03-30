using System;
using System.Collections.Generic;

namespace Website.Models.DTOs.PropertyImage
{
    public class PropertyImagesViewModel
    {
        public Guid PropertyId { get; set; }
        public IEnumerable<Models.PropertyImage> PropertyImages { get; set; }
    }
}

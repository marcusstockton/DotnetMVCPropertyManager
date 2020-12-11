using Microsoft.AspNetCore.Http;
using System;

namespace Website.Models.DTOs.PropertyImage
{
    public class PropertyImageCreateDto
    {
        public IFormFile Image { get; set; }
        public Guid PropertyId { get; set; }
    }
}
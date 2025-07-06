using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.PropertyImage
{
    public class PropertyImageCreateDto
    {
        public IFormFile Image { get; set; }
        public Guid PropertyId { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.DTOs.Tenants;

namespace Website.Models.DTOs.Properties
{
    public class PropertyDetailDTO
    {
        public PropertyDetailDTO()
        {
            Documents = new List<PropertyDocument> { };
        }
        public Guid Id { get; set; }

        [Display(Name = "Date Purchased"), DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DataType(DataType.Currency), Display(Name = "Property Value")]
        public double PropertyValue { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public Guid AddressId { get; set; }
        public virtual AddressDTO Address { get; set; }
        public virtual List<TenantDTO> Tenants { get; set; }
        public virtual List<PropertyDocument> Documents { get; set; }
        public virtual List<PropertyImage> Images { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Date Updated"), DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
    }
}

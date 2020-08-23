using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Currency), Display(Name = "Property Value"), Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double PropertyValue { get; set; }

        [Range(1, 100, ErrorMessage = "You cannot have a property with more than 100 rooms.")]
        public int NoOfRooms { get; set; }

        [Range(1, 10000, ErrorMessage = "You cannot charge more than 10000 per month.")]
        public double MonthlyRentAmount { get; set; }

        [MaxLength(1000), Description("Describe the property")]
        public string Description { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public Guid AddressId { get; set; }
        public string AddressString
        {
            get { return $"{Address.Line1} {Address.Line2} {Address.Line3} {Address.Postcode} {Address.Town} {Address.City}"; }
        }

        public virtual AddressDTO Address { get; set; }
        public virtual List<TenantDTO> Tenants { get; set; }
        public virtual List<PropertyDocument> Documents { get; set; }
        public virtual List<PropertyImage> Images { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Date Updated"), DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
    }
}

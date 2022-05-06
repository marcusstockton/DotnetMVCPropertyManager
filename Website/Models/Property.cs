using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Property : Base
    {
        public Property()
        {
            Documents = new List<PropertyDocument> { };
        }

        public DateTimeOffset PurchaseDate { get; set; }

        [Range(1, 100)]
        public int NoOfRooms { get; set; }

        public double MonthlyRentAmount { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public double PropertyValue { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Tenant> Tenants { get; set; }
        public virtual List<PropertyDocument> Documents { get; set; }
        public virtual List<PropertyImage> Images { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Website.Models
{
    public class Property : Base
    {
        public DateTime PurchaseDate { get; set; }
        public double PropertyValue { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Tenant> Tenants { get; set; }
        public virtual List<PropertyDocument> Documents { get; set; }
        public virtual List<PropertyImage> Images { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Website.Models.DTOs.Properties;

namespace Website.Models.DTOs.Portfolios
{
    public class PortfolioDetailsDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Display(Name = "# Properties")]
        public int NumberOfProperties
        { get { return (Properties == null) ? 0 :  Properties.Count(); } }

        [Display(Name = "Total Property Value")]
        public double TotalPropertyValue
        { get { return (Properties == null) ? 0 : Properties.Select(x => x.PropertyValue).Sum(); } }

        [Display(Name = "Gross Income")]
        public double GrossIncome
        { get { return (Properties == null) ? 0 : Properties.Select(x => x.MonthlyRentAmount).Sum(); } }

        public string Name { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual IList<PropertyListDTO> Properties { get; set; }
    }
}
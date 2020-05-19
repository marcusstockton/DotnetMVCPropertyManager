using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.DTOs.Properties;

namespace Website.Models.DTOs.Portfolios
{
    public class PortfolioDetailsDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Display(Name="# Properties")]
        public int NumberOfProperties { get { return Properties.Count(); } }
        public double GrossIncome { get { return Properties.Select(x => x.MonthlyRentAmount).Sum(); } }
        public string Name { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual IList<PropertyDetailDTO> Properties { get; set; }
    }
}

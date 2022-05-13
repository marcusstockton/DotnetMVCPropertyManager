using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Portfolios
{
    public class PortfolioAdminIndexDto
    {
        public Guid Id { get; set; }
        [Display(Name="Portfolio Name")]
        public string Name { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Created Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTimeOffset CreatedDate { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Updated Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}

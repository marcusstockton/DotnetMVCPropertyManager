using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public abstract class Base
    {
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
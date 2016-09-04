using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi0904.Models
{
    public class ProductVM : IValidatableObject
    {
        [Required]
        public decimal? Price { get; set; }

        [Required]

        public decimal? Stock { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
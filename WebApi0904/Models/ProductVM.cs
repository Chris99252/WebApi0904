using System.ComponentModel.DataAnnotations;

namespace WebApi0904.Models
{
    public class ProductVM
    {
        [Required]
        public decimal? Price { get; set; }

        [Required]

        public decimal? Stock { get; set; }
    }
}
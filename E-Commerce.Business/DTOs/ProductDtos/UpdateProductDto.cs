using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be minimum 2, maximum 100 characters long.")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Stock cannot be a negative value!")]
        public int StockQuantity { get; set; }
    }
}

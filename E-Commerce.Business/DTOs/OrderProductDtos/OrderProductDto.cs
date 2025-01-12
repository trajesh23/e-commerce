using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Business.DTOs.OrderProductDtos
{
    public class OrderProductDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        [DefaultValue(0)]
        public int Quantity { get; set; }
    }
}

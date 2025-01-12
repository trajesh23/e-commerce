using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

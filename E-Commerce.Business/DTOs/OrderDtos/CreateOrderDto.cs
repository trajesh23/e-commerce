using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date time.")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be at least 0.01.")]
        [DefaultValue(0.01)]
        public decimal TotalAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}

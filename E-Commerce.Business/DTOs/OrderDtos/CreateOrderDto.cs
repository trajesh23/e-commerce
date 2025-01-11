using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public ICollection<OrderProductDto> OrderProducts { get; set; } = [];
    }
}

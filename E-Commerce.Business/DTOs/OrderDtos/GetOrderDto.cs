using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Business.DTOs.OrderProductDtos;

namespace E_Commerce.Business.DTOs.OrderDtos
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}

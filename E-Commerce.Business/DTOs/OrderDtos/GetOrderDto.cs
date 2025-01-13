using E_Commerce.Business.DTOs.OrderProductDtos;

namespace E_Commerce.Business.DTOs.OrderDtos
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerId { get; set; }
        public ICollection<OrderProductDto> OrderProducts { get; set; }
    }
}

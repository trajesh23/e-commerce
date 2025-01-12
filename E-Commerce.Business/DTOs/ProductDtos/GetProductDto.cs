namespace E_Commerce.Business.DTOs.ProductDtos
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}

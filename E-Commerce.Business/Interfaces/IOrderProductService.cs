using E_Commerce.Business.DTOs.OrderProductDtos;

namespace E_Commerce.Business.Interfaces
{
    public interface IOrderProductService
    {
        Task<OrderProductDto> GetByIdAsync(int id);      // Gets data by ID.
    }
}

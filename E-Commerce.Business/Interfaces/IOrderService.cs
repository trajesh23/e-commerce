using E_Commerce.Business.DTOs.OrderDtos;

namespace E_Commerce.Business.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto createOrderDto);        // Create a new resource.
        Task<IEnumerable<GetOrderDto>> GetAllOrdersAsync(); // Gets all data.
        Task<GetOrderDto> GetOrderByIdAsync(int id);      // Gets data by ID.
        Task UpdateAsync(int id, UpdateOrderDto updateOrderDto);        // Updates existing data.
        Task DeleteOrderByIdAsync(int id);      // Deletes data by ID.
    }
}

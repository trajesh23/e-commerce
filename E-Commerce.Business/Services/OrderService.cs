using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var newOrder = _mapper.Map<Order>(createOrderDto);
            await _orderRepository.CreateAsync(newOrder);
            await SaveChangesAsync("Order creation failed.");

            return newOrder.Id;
        }

        public async Task DeleteOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            await _orderRepository.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete order.");
        }

        public async Task<IEnumerable<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            return _mapper.Map<GetOrderDto>(order);   
        }

        public async Task UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            _mapper.Map(updateOrderDto, order);
            await _orderRepository.UpdateAsync(order);
            await SaveChangesAsync("Failed to update order.");
        }

        // Private helper method to handle save changes with consistent error handling
        private async Task SaveChangesAsync(string errorMessage)
        {
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{errorMessage}. Details: {ex.Message}");
            }
        }
    }
}

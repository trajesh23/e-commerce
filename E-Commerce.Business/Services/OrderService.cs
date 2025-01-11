using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.DataAccess.Respositories.Interfaces;
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

        public OrderService(IOrderRepository orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var newOrder = _mapper.Map<Order>(createOrderDto);
            await _orderRepository.CreateAsync(newOrder);
        }

        public async Task DeleteOrderByIdAsync(int id)
        {
            await _orderRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return _mapper.Map<GetOrderDto>(order);
        }

        public async Task UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new Exception("Order not found");

            _mapper.Map(updateOrderDto, order);

            await _orderRepository.UpdateAsync(order);
        }
    }
}

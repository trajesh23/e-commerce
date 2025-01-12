using AutoMapper;
using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;

namespace E_Commerce.Business.Services
{
    public class OrderProductService : IOrderProductService
    {
        private readonly IOrderProductRepository _orderProductService;
        private readonly IMapper _mapper;

        public OrderProductService(IOrderProductRepository orderProductService, IMapper mapper)
        {
            _orderProductService = orderProductService;
            _mapper = mapper;
        }

        public async Task<OrderProductDto> GetByIdAsync(int id)
        {
            var orderProduct = await _orderProductService.GetByIdAsync(id);

            if (orderProduct == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            return _mapper.Map<OrderProductDto>(orderProduct);
        }
    }
}

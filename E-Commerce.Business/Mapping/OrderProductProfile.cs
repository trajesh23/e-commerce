using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Business.Mapping
{
    public class OrderProductProfile : UserProfile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProductDto, OrderProduct>().ReverseMap();    
        }
    }
}

using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Business.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>()
                    .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => new List<OrderProduct>()));

            CreateMap<UpdateOrderDto, Order>().ReverseMap();
            CreateMap<GetOrderDto, Order>().ReverseMap();
        }
    }
}

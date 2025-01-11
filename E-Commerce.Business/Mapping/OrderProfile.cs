using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>().ReverseMap();
            CreateMap<UpdateOrderDto, Order>().ReverseMap();
            CreateMap<GetOrderDto, Order>().ReverseMap();
        }
    }
}

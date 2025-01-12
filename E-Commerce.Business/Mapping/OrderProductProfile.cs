using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

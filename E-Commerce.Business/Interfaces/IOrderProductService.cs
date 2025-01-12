using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Business.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Interfaces
{
    public interface IOrderProductService
    {
        Task<OrderProductDto> GetByIdAsync(int id);      // Gets data by ID.
    }
}

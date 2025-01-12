using AutoMapper;
using E_Commerce.Business.DTOs.ProductDtos;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Business.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
            CreateMap<GetProductDto, Product>().ReverseMap();
        }
    }
}

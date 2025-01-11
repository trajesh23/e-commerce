using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.ProductDtos;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateProductDto createProductDto)
        {
            var newProduct = _mapper.Map<Product>(createProductDto);
            await _productRepository.CreateAsync(newProduct);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _productRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<GetProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<GetProductDto>>(products);
        }

        public async Task<GetProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            return _mapper.Map<GetProductDto>(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDto entity)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            _mapper.Map(entity, product);

            await _productRepository.UpdateAsync(product);
        }
    }
}

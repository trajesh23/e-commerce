using E_Commerce.API.Filters;
using E_Commerce.Business.DTOs.ProductDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.Business.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetProductsAsync()
        {
            var products = await _productService.GetAllAsync();

            return Ok(new ServiceMessage<IEnumerable<GetProductDto>>
            {
                IsSucceed = true,
                Count = products.Count(),
                Data = products
            });
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceMessage<GetProductDto>>> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return Ok(new ServiceMessage<GetProductDto>
            {
                IsSucceed = true,
                Data = product
            });
        }

        // POST: api/Products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto newProduct)
        {
            await _productService.CreateAsync(newProduct);

            return Ok(new ServiceMessage<CreateProductDto>
            {
                IsSucceed = true,
                Message = $"Product successfully created.",
                Data = newProduct
            });
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto newProduct)
        {
            await _productService.UpdateAsync(id, newProduct);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"Product with '{id}' id successfully updated."
            });
        }

        // DELETE: api/Products/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _productService.DeleteByIdAsync(id);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"Product with '{id}' id successfully deleted."
            });
        }
    }
}


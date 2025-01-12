using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.Business.Types;
using E_Commerce.DataAccess.Respositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(new ServiceMessage<IEnumerable<GetOrderDto>>
            {
                IsSucceed = true,
                Count = orders.Count(),
                Data = orders
            });
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceMessage<GetOrderDto>>> GetOrderByIdAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            return Ok(new ServiceMessage<GetOrderDto>
            {
                IsSucceed = true,
                Data = order
            });
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto newOrder)
        {
            await _orderService.CreateOrderAsync(newOrder);

            return Ok(new ServiceMessage<CreateOrderDto>
            {
                IsSucceed = true,
                Message = $"Order successfully created.",
                Data = newOrder
            });
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderToUpdate)
        {
            await _orderService.UpdateAsync(id, orderToUpdate);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"Order with '{id}' id successfully updated."
            });
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            await _orderService.DeleteOrderByIdAsync(id);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"Order with '{id}' id successfully deleted."
            });
        }
    }
}

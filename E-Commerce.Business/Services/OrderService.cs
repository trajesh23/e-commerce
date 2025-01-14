using AutoMapper;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using System.Data;

namespace E_Commerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var newOrder = _mapper.Map<Order>(createOrderDto);

                decimal totalAmount = 0;

                foreach (var orderProductDto in createOrderDto.OrderProducts)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(orderProductDto.ProductId);

                    // Check if product exists
                    if (product == null)
                        throw new KeyNotFoundException($"Product with ID {orderProductDto.ProductId} not found.");

                    // Check if stock quantity is less than requested quantity
                    if (product.StockQuantity < orderProductDto.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product with ID {orderProductDto.ProductId}.");
                    
                    // Decrease stock after order
                    product.StockQuantity -= orderProductDto.Quantity;

                    // Decrease stock
                    await _unitOfWork.Products.UpdateAsync(product);

                    // Calculate total amount
                    totalAmount += product.Price * orderProductDto.Quantity;

                    // Add product id and quantity to order
                    newOrder.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = orderProductDto.ProductId,
                        Quantity = orderProductDto.Quantity
                    });

                    // Set order time 
                    newOrder.OrderDate = DateTime.Now;
                }

                // Calculated amount equals order amount
                newOrder.TotalAmount = totalAmount;
                await _orderRepository.CreateAsync(newOrder);

                // Save all changes
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync(); // Rollback transaction in an emergency
                throw new Exception("An error occurred while creating an order. All operations rolling back.");
            }
        }


        public async Task DeleteOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            // Return all stock back when an order deleted
            foreach (var orderProduct in order.OrderProducts)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(orderProduct.ProductId);

                if (product != null)
                {
                    product.StockQuantity += orderProduct.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product);
                }
            }

            await _orderRepository.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete order.");
        }

        public async Task<IEnumerable<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto> GetOrderByIdAsync(int id)
        {
            // Order'ı ve ilişkili OrderProducts'ı çekiyoruz
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            return new GetOrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                }).ToList(),
                TotalAmount = order.TotalAmount,
            };
        }

        public async Task UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Bring all units back to stock
                foreach (var orderProduct in order.OrderProducts)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(orderProduct.ProductId);

                    if (product != null)
                    {
                        product.StockQuantity += orderProduct.Quantity; // Add stock back to product table
                        await _unitOfWork.Products.UpdateAsync(product);
                    }
                }

                order.OrderProducts.Clear(); // Clear products in order

                decimal totalAmount = 0; // Set total amount to zero

                // Take all products in the order 
                foreach (var orderProductDto in updateOrderDto.OrderProducts)
                {
                    // Get the product from Products table
                    var product = await _unitOfWork.Products.GetByIdAsync(orderProductDto.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Product with ID {orderProductDto.ProductId} not found.");

                    // Check if stock is adequate
                    if (product.StockQuantity < orderProductDto.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product with ID {orderProductDto.ProductId}.");

                    // Update stock quantity
                    product.StockQuantity -= orderProductDto.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product);

                    // Add new products to order
                    order.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = orderProductDto.ProductId,
                        Quantity = orderProductDto.Quantity,
                        OrderModifiedDate = DateTime.Now,
                    });

                    // Calculate new price
                    totalAmount += product.Price * orderProductDto.Quantity;
                }

                // Update order information
                order.TotalAmount = totalAmount;
                order.CustomerId = updateOrderDto.CustomerId;
                order.ModifiedDate = DateTime.Now;

                await _unitOfWork.Orders.UpdateAsync(order);

                // Save changes
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Rollback in case of an error
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while updating the order. All operations are rolling back.");
            }
        }

        // Private helper method to handle save changes with consistent error handling
        private async Task SaveChangesAsync(string errorMessage)
        {
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{errorMessage}. Details: {ex.Message}");
            }
        }
    }
}

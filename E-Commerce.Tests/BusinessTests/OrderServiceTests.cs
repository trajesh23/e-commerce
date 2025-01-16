using AutoMapper;
using Moq;
using E_Commerce.Business.DTOs.OrderDtos;
using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Business.Services;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commerce.Tests.BusinessTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder_WhenDataIsValid()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = "1",
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto { ProductId = 1, Quantity = 2 },
                new OrderProductDto { ProductId = 2, Quantity = 3 }
            }
            };

            var products = new List<Product>
        {
            new Product { Id = 1, StockQuantity = 10, Price = 50 },
            new Product { Id = 2, StockQuantity = 20, Price = 30 }
        };

            var newOrder = new Order
            {
                CustomerId = "1",
                OrderProducts = new List<OrderProduct>()
            };

            var mockTransaction = new Mock<IDbContextTransaction>();

            _mapperMock.Setup(m => m.Map<Order>(createOrderDto))
                        .Returns(newOrder);

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((int id) => products.FirstOrDefault(p => p.Id == id));

            _unitOfWorkMock.Setup(u => u.Products.UpdateAsync(It.IsAny<Product>()))
                            .Returns(Task.CompletedTask);

            _orderRepositoryMock.Setup(o => o.CreateAsync(It.IsAny<Order>()))
                                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync())
                            .ReturnsAsync(mockTransaction.Object);

            // Act
            await _orderService.CreateOrderAsync(createOrderDto);

            // Assert
            _mapperMock.Verify(m => m.Map<Order>(createOrderDto), Times.Once);
            _unitOfWorkMock.Verify(u => u.Products.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _orderRepositoryMock.Verify(o => o.CreateAsync(It.IsAny<Order>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = "1",
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto { ProductId = 1, Quantity = 2 }
            }
            };

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(1))
                            .ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NullReferenceException>(() =>
                _orderService.CreateOrderAsync(createOrderDto));
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order
            {
                Id = orderId,
                OrderProducts = new List<OrderProduct>
            {
                new OrderProduct { ProductId = 1, Quantity = 2 },
                new OrderProduct { ProductId = 2, Quantity = 3 }
            }
            };

            var products = new List<Product>
        {
            new Product { Id = 1, StockQuantity = 5 },
            new Product { Id = 2, StockQuantity = 10 }
        };

            _orderRepositoryMock.Setup(o => o.GetByIdAsync(orderId))
                                .ReturnsAsync(order);

            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((int id) => products.FirstOrDefault(p => p.Id == id));

            _orderRepositoryMock.Setup(o => o.DeleteByIdAsync(orderId))
                                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            _orderRepositoryMock.Verify(o => o.GetByIdAsync(orderId), Times.Once);
            _orderRepositoryMock.Verify(o => o.DeleteByIdAsync(orderId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Products.UpdateAsync(It.IsAny<Product>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_ShouldThrowKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;

            _orderRepositoryMock.Setup(o => o.GetByIdAsync(orderId))
                                .ReturnsAsync((Order)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _orderService.DeleteOrderByIdAsync(orderId));

            Assert.Equal($"Order with id '{orderId}' not found.", exception.Message);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;

            var order = new Order
            {
                Id = orderId,
                CustomerId = "1",
                OrderDate = DateTime.Now,
                TotalAmount = 100,
                OrderProducts = new List<OrderProduct>
            {
                new OrderProduct { ProductId = 1, Quantity = 2 },
                new OrderProduct { ProductId = 2, Quantity = 3 }
            }
            };

            var getOrderDto = new GetOrderDto
            {
                Id = orderId,
                CustomerId = "1",
                TotalAmount = 100,
                OrderProducts = new List<OrderProductDto>
            {
                new OrderProductDto { ProductId = 1, Quantity = 2 },
                new OrderProductDto { ProductId = 2, Quantity = 3 }
            }
            };

            _orderRepositoryMock.Setup(o => o.GetByIdAsync(orderId))
                                .ReturnsAsync(order);

            _mapperMock.Setup(m => m.Map<GetOrderDto>(order))
                        .Returns(getOrderDto);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getOrderDto.Id, result.Id);
            Assert.Equal(getOrderDto.TotalAmount, result.TotalAmount);
            _orderRepositoryMock.Verify(o => o.GetByIdAsync(orderId), Times.Once);
        }
    }
}



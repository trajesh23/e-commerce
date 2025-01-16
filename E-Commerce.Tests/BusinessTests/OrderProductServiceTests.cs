using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using E_Commerce.Business.DTOs.OrderProductDtos;
using E_Commerce.Domain.Entities;
using E_Commerce.Business.Services;
using E_Commerce.DataAccess.Respositories.Interfaces;

namespace E_Commerce.Tests.BusinessTests
{
    public class OrderProductServiceTests
    {
        private readonly Mock<IOrderProductRepository> _orderProductRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderProductService _orderProductService;

        public OrderProductServiceTests()
        {
            _orderProductRepositoryMock = new Mock<IOrderProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _orderProductService = new OrderProductService(_orderProductRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrderProductDto_WhenOrderProductExists()
        {
            // Arrange
            var orderProductId = 1;
            var orderProduct = new OrderProduct
            {
                OrderId = 1,
                ProductId = 2,
                Quantity = 5
            };
            var orderProductDto = new OrderProductDto
            {
                ProductId = 2,
                Quantity = 5
            };

            _orderProductRepositoryMock
                .Setup(repo => repo.GetByIdAsync(orderProductId))
                .ReturnsAsync(orderProduct);

            _mapperMock
                .Setup(mapper => mapper.Map<OrderProductDto>(orderProduct))
                .Returns(orderProductDto);

            // Act
            var result = await _orderProductService.GetByIdAsync(orderProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderProductDto.ProductId, result.ProductId);
            Assert.Equal(orderProductDto.Quantity, result.Quantity);

            _orderProductRepositoryMock.Verify(repo => repo.GetByIdAsync(orderProductId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<OrderProductDto>(orderProduct), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenOrderProductDoesNotExist()
        {
            // Arrange
            var orderProductId = 99;

            _orderProductRepositoryMock
                .Setup(repo => repo.GetByIdAsync(orderProductId))
                .ReturnsAsync((OrderProduct)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _orderProductService.GetByIdAsync(orderProductId));

            Assert.Equal($"Order with id '{orderProductId}' not found.", exception.Message);

            _orderProductRepositoryMock.Verify(repo => repo.GetByIdAsync(orderProductId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<OrderProductDto>(It.IsAny<OrderProduct>()), Times.Never);
        }
    }
}
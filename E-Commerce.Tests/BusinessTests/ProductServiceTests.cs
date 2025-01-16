using AutoMapper;
using E_Commerce.Business.DTOs.ProductDtos;
using E_Commerce.Business.Services;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace E_Commerce.Tests.BusinessTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct_WhenDataIsValid()
        {
            // Arrange
            var createProductDto = new CreateProductDto
            {
                ProductName = "Test Product",
                Price = 100,
                StockQuantity = 50
            };

            var product = new Product
            {
                ProductName = "Test Product",
                Price = 100,
                StockQuantity = 50
            };

            _mapperMock.Setup(m => m.Map<Product>(createProductDto))
                       .Returns(product);

            _productRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
                                  .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            // Act
            await _productService.CreateAsync(createProductDto);

            // Assert
            _mapperMock.Verify(m => m.Map<Product>(createProductDto), Times.Once);
            _productRepositoryMock.Verify(repo => repo.CreateAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            var product = new Product
            {
                Id = productId,
                ProductName = "Test Product",
                Price = 100,
                StockQuantity = 50
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.DeleteByIdAsync(productId))
                                  .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteByIdAsync(productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.DeleteByIdAsync(productId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _productService.DeleteByIdAsync(productId));

            Assert.Equal($"Product with id '{productId}' not found.", exception.Message);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.DeleteByIdAsync(It.IsAny<int>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            var product = new Product
            {
                Id = productId,
                ProductName = "Test Product",
                Price = 100,
                StockQuantity = 50
            };

            var getProductDto = new GetProductDto
            {
                Id = productId,
                ProductName = "Test Product",
                Price = 100,
                StockQuantity = 50
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync(product);

            _mapperMock.Setup(m => m.Map<GetProductDto>(product))
                       .Returns(getProductDto);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getProductDto.Id, result.Id);
            Assert.Equal(getProductDto.ProductName, result.ProductName);
            Assert.Equal(getProductDto.Price, result.Price);
            Assert.Equal(getProductDto.StockQuantity, result.StockQuantity);

            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetProductDto>(product), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _productService.GetByIdAsync(productId));

            Assert.Equal($"Product with id '{productId}' not found.", exception.Message);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            var product = new Product
            {
                Id = productId,
                ProductName = "Old Product",
                Price = 50,
                StockQuantity = 20
            };

            var updateProductDto = new UpdateProductDto
            {
                ProductName = "Updated Product",
                Price = 100,
                StockQuantity = 50
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(product))
                                  .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                           .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map(updateProductDto, product))
                       .Callback<UpdateProductDto, Product>((src, dest) =>
                       {
                           dest.ProductName = src.ProductName;
                           dest.Price = src.Price;
                           dest.StockQuantity = src.StockQuantity;
                       });

            // Act
            await _productService.UpdateAsync(productId, updateProductDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);

            Assert.Equal(updateProductDto.ProductName, product.ProductName);
            Assert.Equal(updateProductDto.Price, product.Price);
            Assert.Equal(updateProductDto.StockQuantity, product.StockQuantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            var updateProductDto = new UpdateProductDto
            {
                ProductName = "Updated Product",
                Price = 100,
                StockQuantity = 50
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _productService.UpdateAsync(productId, updateProductDto));

            Assert.Equal("Product not found", exception.Message);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}

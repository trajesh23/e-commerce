using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace E_Commerce.Tests.DataAccessTests
{
    public class OrderRepositoryTests
    {
        private readonly EcommerceContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            // In-memory database options
            var options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new EcommerceContext(options);
            _repository = new OrderRepository(_context);
            _context.Database.EnsureDeleted(); // Clear database before tests
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddOrderToDatabase()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = Guid.NewGuid().ToString(),
                OrderProducts = new List<OrderProduct>
            {
                new OrderProduct { ProductId = 3, Quantity = 2 }
            }
            };

            // Act
            await _repository.CreateAsync(order);

            // Assert
            var addedOrder = await _context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.NotNull(addedOrder);
            Assert.Equal(order.CustomerId, addedOrder.CustomerId);
            Assert.Single(addedOrder.OrderProducts);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrderWithProducts()
        {
            // Arrange
            var order = new Order
            {
                Id = 2,
                CustomerId = Guid.NewGuid().ToString(),
                OrderProducts = new List<OrderProduct>
            {
                new OrderProduct { ProductId = 1, Quantity = 3 },
                new OrderProduct { ProductId = 2, Quantity = 1 }
            }
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.CustomerId, result.CustomerId);
            Assert.Equal(2, result.OrderProducts.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { Id = 3, CustomerId = Guid.NewGuid().ToString() },
            new Order { Id = 4, CustomerId = Guid.NewGuid().ToString() }
        };
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 5,
                CustomerId = Guid.NewGuid().ToString()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            order.CustomerId = Guid.NewGuid().ToString(); // Güncelleme işlemi
            await _repository.UpdateAsync(order);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.NotNull(updatedOrder);
            Assert.Equal(order.CustomerId, updatedOrder.CustomerId);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 6,
                CustomerId = Guid.NewGuid().ToString()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteByIdAsync(order.Id);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Null(deletedOrder);
        }
    }
}
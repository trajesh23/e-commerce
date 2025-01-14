using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductRepositoryTests
{
    private EcommerceContext CreateNewContext()
    {
        var options = new DbContextOptionsBuilder<EcommerceContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Different in memory db for every test
            .Options;

        return new EcommerceContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProductToDatabase()
    {
        // Arrange
        using var context = CreateNewContext();
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 100,
            StockQuantity = 10,
            ModifiedDate = DateTime.Now
        };

        // Act
        await repository.CreateAsync(product);

        // Assert
        var addedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(addedProduct);
        Assert.Equal("Test Product", addedProduct.ProductName);
        Assert.Equal(100, addedProduct.Price);
        Assert.Equal(10, addedProduct.StockQuantity);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProductById()
    {
        // Arrange
        using var context = CreateNewContext();
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = 2,
            ProductName = "Test Product 2",
            Price = 150,
            StockQuantity = 5,
            ModifiedDate = DateTime.Now
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var retrievedProduct = await repository.GetByIdAsync(product.Id);

        // Assert
        Assert.NotNull(retrievedProduct);
        Assert.Equal("Test Product 2", retrievedProduct.ProductName);
        Assert.Equal(150, retrievedProduct.Price);
        Assert.Equal(5, retrievedProduct.StockQuantity);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        using var context = CreateNewContext();
        var repository = new ProductRepository(context);

        var products = new List<Product>
        {
            new Product { Id = 3, ProductName = "Product 1", Price = 50, StockQuantity = 20 },
            new Product { Id = 4, ProductName = "Product 2", Price = 75, StockQuantity = 15 }
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductDetails()
    {
        // Arrange
        using var context = CreateNewContext();
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = 5,
            ProductName = "Old Product Name",
            Price = 200,
            StockQuantity = 30,
            ModifiedDate = DateTime.Now
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        product.ProductName = "Updated Product Name";
        product.Price = 250;
        await repository.UpdateAsync(product);

        // Assert
        var updatedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product Name", updatedProduct.ProductName);
        Assert.Equal(250, updatedProduct.Price);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveProductFromDatabase()
    {
        // Arrange
        using var context = CreateNewContext();
        var repository = new ProductRepository(context);

        var product = new Product
        {
            Id = 6,
            ProductName = "Product to Delete",
            Price = 300,
            StockQuantity = 10,
            ModifiedDate = DateTime.Now
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteByIdAsync(product.Id);

        // Assert
        var deletedProduct = await context.Products.FindAsync(product.Id);
        Assert.Null(deletedProduct);
    }
}


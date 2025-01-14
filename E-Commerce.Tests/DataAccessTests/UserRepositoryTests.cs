using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Tests.DataAccessTests
{
    public class UserRepositoryTests
    {
        private EcommerceContext CreateNewContext()
        {
            var options = new DbContextOptionsBuilder<EcommerceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Different in memory db for every test
                .Options;

            return new EcommerceContext(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            using var context = CreateNewContext();
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = "1",
                UserName = "TestUser",
                Email = "test@example.com"
            };

            // Act
            await repository.CreateAsync(user);

            // Assert
            var addedUser = await context.Users.FindAsync(user.Id);
            Assert.NotNull(addedUser);
            Assert.Equal("TestUser", addedUser.UserName);
            Assert.Equal("test@example.com", addedUser.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserById()
        {
            // Arrange
            using var context = CreateNewContext();
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = "2",
                UserName = "TestUser2",
                Email = "test2@example.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var retrievedUser = await repository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal("TestUser2", retrievedUser.UserName);
            Assert.Equal("test2@example.com", retrievedUser.Email);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            using var context = CreateNewContext();
            var repository = new UserRepository(context);

            var users = new List<User>
        {
            new User { Id = "3", UserName = "User1", Email = "user1@example.com" },
            new User { Id = "4", UserName = "User2", Email = "user2@example.com" }
        };
            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserDetails()
        {
            // Arrange
            using var context = CreateNewContext();
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = "5",
                UserName = "OldUserName",
                Email = "old@example.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            user.UserName = "UpdatedUserName";
            user.Email = "updated@example.com";
            await repository.UpdateAsync(user);

            // Assert
            var updatedUser = await context.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdatedUserName", updatedUser.UserName);
            Assert.Equal("updated@example.com", updatedUser.Email);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            using var context = CreateNewContext();
            var repository = new UserRepository(context);

            var user = new User
            {
                Id = "6",
                UserName = "UserToDelete",
                Email = "delete@example.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteByIdAsync(user.Id);

            // Assert
            var deletedUser = await context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }
    }

}


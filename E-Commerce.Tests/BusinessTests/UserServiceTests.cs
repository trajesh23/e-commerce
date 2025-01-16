using AutoMapper;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace E_Commerce.Tests.BusinessTests
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenDataIsValid()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                Role = UserRole.Customer
            };

            var newUser = new User
            {
                Email = "test@example.com",
                UserName = "test",
                Role = UserRole.Customer
            };

            _mapperMock.Setup(m => m.Map<User>(createUserDto))
                       .Returns(newUser);

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.CreateUserAsync(createUserDto);

            // Assert
            _mapperMock.Verify(m => m.Map<User>(createUserDto), Times.Once);
            _userManagerMock.Verify(um => um.CreateAsync(It.Is<User>(u => u.Email == createUserDto.Email), createUserDto.Password), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowException_WhenUserCreationFails()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                Role = UserRole.Customer
            };

            var newUser = new User
            {
                Email = "test@example.com",
                UserName = "test",
                Role = UserRole.Customer
            };

            _mapperMock.Setup(m => m.Map<User>(createUserDto))
                       .Returns(newUser);

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email is already taken." }));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.CreateUserAsync(createUserDto));

            Assert.Equal("User creation failed: Email is already taken.", exception.Message);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, Email = "test@example.com" };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(um => um.DeleteAsync(It.IsAny<User>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.DeleteUserByIdAsync(userId);

            // Assert
            _userManagerMock.Verify(um => um.FindByIdAsync(userId), Times.Once);
            _userManagerMock.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = "1";

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync((User)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.DeleteUserByIdAsync(userId));

            Assert.Equal($"User with id '{userId}' not found.", exception.Message);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, Email = "test@example.com" };
            var getUserDto = new GetUserDto { Id = userId, Email = "test@example.com" };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<GetUserDto>(user))
                       .Returns(getUserDto);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(getUserDto.Id, result.Id);
            Assert.Equal(getUserDto.Email, result.Email);

            _userManagerMock.Verify(um => um.FindByIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetUserDto>(user), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = "1";

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync((User)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.GetUserByIdAsync(userId));

            Assert.Equal($"User with id '{userId}' not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var updateUserDto = new UpdateUserDto { Email = "updated@example.com" };
            var user = new User { Id = userId, Email = "test@example.com" };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map(updateUserDto, user))
                       .Callback<UpdateUserDto, User>((src, dest) => dest.Email = src.Email);

            _userManagerMock.Setup(um => um.UpdateAsync(user))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Equal(updateUserDto.Email, user.Email);
            _userManagerMock.Verify(um => um.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange
            var userId = "1";
            var updateUserDto = new UpdateUserDto { Email = "updated@example.com" };
            var user = new User { Id = userId, Email = "test@example.com" };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(um => um.UpdateAsync(user))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed." }));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.UpdateUserAsync(userId, updateUserDto));

            Assert.Equal("Failed to update user: Update failed.", exception.Message);
        }
    }
}

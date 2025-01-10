using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Interfaces
{
    public interface IUserService 
    {
        Task CreateUserAsync(CreateUserDto createUserDto);
        Task<GetUserDto> GetAllUsersAsync();
        Task<GetUserDto> GetUserByIdAsync(int id);
        Task UpdateUserAsync(UpdateUserDto updateUserDto);
        Task DeleteUserByIdAsync(int id);
    }
}

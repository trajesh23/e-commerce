using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Services
{
    public class UserService : IUserService
    {
        public Task CreateUserAsync(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GetUserDto> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetUserDto> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
    }
}

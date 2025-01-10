using AutoMapper;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserDto createUserDto)
        {
            var newUser = _mapper.Map<User>(createUserDto);
            await _userRepository.CreateAsync(newUser);
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            await _userRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetUserDto>>(users);
        }

        public async Task<GetUserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<GetUserDto>(user);
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            _mapper.Map(updateUserDto, user);

            await _userRepository.UpdateAsync(user);
        }
    }
}

using E_Commerce.Business.DTOs.UserDtos;

namespace E_Commerce.Business.Interfaces
{
    public interface IUserService 
    {
        Task CreateUserAsync(CreateUserDto createUserDto);
        Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
        Task<GetUserDto> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteUserByIdAsync(int id);
    }
}

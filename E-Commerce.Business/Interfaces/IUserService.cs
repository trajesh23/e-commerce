using E_Commerce.Business.DTOs.UserDtos;

namespace E_Commerce.Business.Interfaces
{
    public interface IUserService 
    {
        Task CreateUserAsync(CreateUserDto createUserDto);
        Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
        Task<GetUserDto> GetUserByIdAsync(string id);
        Task UpdateUserAsync(string id, UpdateUserDto updateUserDto);
        Task DeleteUserByIdAsync(string id);
    }
}

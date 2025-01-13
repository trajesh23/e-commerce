using AutoMapper;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task CreateUserAsync(CreateUserDto createUserDto)
    {
        var newUser = _mapper.Map<User>(createUserDto);

        // Kullanıcı adı boş ise e-posta adresini kullanıcı adı olarak ayarla
        newUser.UserName = createUserDto.Email.Split('@')[0]; // E-posta adresinin '@' öncesi kısmını kullan

        // Varsayılan rol ata
        newUser.Role = createUserDto.Role;

        // Kullanıcı oluştur ve şifreyi hashle
        var result = await _userManager.CreateAsync(newUser, createUserDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {errors}");
        }
    }


    public async Task DeleteUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to delete user: {errors}");
        }
    }

    public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        return _mapper.Map<IEnumerable<GetUserDto>>(users);
    }

    public async Task<GetUserDto> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        return _mapper.Map<GetUserDto>(user);
    }

    public async Task UpdateUserAsync(string id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        // Update user properties
        _mapper.Map(updateUserDto, user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }
    }
}

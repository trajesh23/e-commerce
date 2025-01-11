using AutoMapper;
using E_Commerce.Business.DataProtection;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDataProtection _protector;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IMapper mapper, IDataProtection protector, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _protector = protector;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateUserAsync(CreateUserDto createUserDto)
    {
        var newUser = _mapper.Map<User>(createUserDto);

        // Password hashing
        newUser.Password = _protector.Protect(createUserDto.Password);

        // Assign default role
        newUser.Role = UserRole.Customer;

        await _userRepository.CreateAsync(newUser);
        await SaveChangesAsync("User creation failed");

        return newUser.Id;
    }

    public async Task DeleteUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        await _userRepository.DeleteByIdAsync(id);
        await SaveChangesAsync("Failed to delete user.");
    }

    public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetUserDto>>(users);
    }

    public async Task<GetUserDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        return _mapper.Map<GetUserDto>(user);
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateAsync(user);
        await SaveChangesAsync("Failed to update user");
    }

    // Private helper method to handle save changes with consistent error handling
    private async Task SaveChangesAsync(string errorMessage)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"{errorMessage}. Details: {ex.Message}");
        }
    }
}

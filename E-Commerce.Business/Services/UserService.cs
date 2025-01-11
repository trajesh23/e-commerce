using AutoMapper;
using E_Commerce.Business.DataProtection;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"User creation failed. {ex.Message.ToList()}");
        }

        return newUser.Id;
    }

    public async Task DeleteUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with id '{id}' not found.");

        await _userRepository.DeleteByIdAsync(id);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete user. {ex.Message.ToList()}");
        }
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

        try
        {
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to update user. {ex.Message.ToList()}");
        }
    }
}

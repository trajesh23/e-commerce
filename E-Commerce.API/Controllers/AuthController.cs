using E_Commerce.Business.DTOs.AuthDtos.LoginDtos;
using E_Commerce.Business.DTOs.AuthDtos.SignUpDtos;
using E_Commerce.Business.Helpers;
using E_Commerce.Business.Types;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthController(UserManager<User> userManager, IConfiguration configuration, JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    // POST: api/Auth/signup
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new user
        var newUser = new User
        {
            FirstName = signUpDto.FirstName,
            LastName = signUpDto.LastName,
            Email = signUpDto.Email,
            PhoneNumber = signUpDto.PhoneNumber,
            UserName = signUpDto.Email,
            Role = UserRole.Customer // Default role is 'customer'
        };

        // Create the user and hash the password
        var result = await _userManager.CreateAsync(newUser, signUpDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { Message = "Sign up failed.", Errors = errors });
        }

        return Ok(new ServiceMessage
        {
            IsSucceed = true,
            Message = "User successfully created."
        });
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return Ok(new ServiceMessage
        {
            IsSucceed = true,
            Message = token
        });
    }
}

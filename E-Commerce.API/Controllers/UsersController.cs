using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Business.Interfaces;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Business.Types;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints required authorization
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(new ServiceMessage<IEnumerable<GetUserDto>>
            {
                IsSucceed = true,
                Count = users.Count(),
                Data = users
            });
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceMessage<GetUserDto>>> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(new ServiceMessage<GetUserDto>
            {
                IsSucceed = true,
                Data = user
            });
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admin
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto newUser)
        {
            await _userService.CreateUserAsync(newUser);

            return Ok(new ServiceMessage<CreateUserDto>
            {
                IsSucceed = true,
                Message = $"User successfully created.",
                Data = newUser
            });
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only admin
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto newUser)
        {
            await _userService.UpdateUserAsync(id, newUser);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"User with '{id}' id successfully updated."
            });
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admin
        public async Task<IActionResult> DeleteUserById(string id)
        {
            await _userService.DeleteUserByIdAsync(id);

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"User with '{id}' id successfully deleted."
            });
        }
    }
}

using Application.DTO_s.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService userService;
		public UserController(IUserService userService)
		{
			this.userService = userService;		
		}
		[HttpPost("create")]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
		{
			var result = await userService.CreateUserAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var result = await userService.LoginAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			var result = await userService.GetUserByIdAsync(id);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
		{
			var result = await userService.SearchUsersAsync(searchTerm);
			return StatusCode(result.StatusCode, result);
		} 

		[HttpGet("all")]
		public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParameter query)
		{
			var result = await userService.GetAllUsersAsync(query);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var result = await userService.ChangePasswordAsync(dto);
			return StatusCode(result.StatusCode, result);
		}
	}
}

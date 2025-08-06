using Application.DTO_s.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

		[HttpGet("By-Id")]
		public async Task<IActionResult> GetUserById([FromQuery] int? memberId)
		{
			var role = User.FindFirst(ClaimTypes.Role).Value;

			if (role == "Admin" && memberId is null)
				return BadRequest("Member ID is required when accessed by an admin.");

			int userId = role == "Admin" ? memberId!.Value
								   : int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

			var result = await userService.GetUserByIdAsync(userId);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("search")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
		public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
		{
			var result = await userService.SearchUsersAsync(searchTerm);
			return StatusCode(result.StatusCode, result);
		} 

		[HttpGet("all")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
		public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParameter query)
		{
			var result = await userService.GetAllUsersAsync(query);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("change-password")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (user is null)
				return Unauthorized("you shoud login to can borrow book");

			var result = await userService.ChangePasswordAsync(int.Parse(user), dto);
			return StatusCode(result.StatusCode, result);
		}
	}
}

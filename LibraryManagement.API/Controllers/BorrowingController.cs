using Application.DTO_s.Borrowing;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BorrowingController : ControllerBase
	{
		private readonly IBorrowingService borrowingService;
		public BorrowingController(IBorrowingService borrowingService)
		{
			this.borrowingService = borrowingService;		
		}
		[HttpPost("borrow")]
		[Authorize(AuthenticationSchemes ="Bearer")]
		public async Task<IActionResult> BorrowBook([FromBody] CreateBorrowingDto dto)
		{
			var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (user is null)
				return Unauthorized("you shoud login to can borrow book");

			var result = await borrowingService.AddBorrowingAsync(int.Parse(user), dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("return")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> ReturnBook([FromBody] ReturnBorrowingDto dto)
		{
			var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (user is null)
				return Unauthorized("you shoud login to can borrow book");

			var result = await borrowingService.ReturnBorrowingAsync(int.Parse(user), dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("history")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> GetMemberBorrowingHistory([FromQuery] int? memberId)
		{
			var role = User.FindFirst(ClaimTypes.Role).Value;

			if (role == "Admin" && memberId is null)
				return BadRequest("Member ID is required when accessed by an admin.");

			int userId = role == "Admin" ? memberId!.Value
                           		: int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			
			var result = await borrowingService.GetMemberBorrowingHistoryAsync(userId);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("current")]
		public async Task<IActionResult> GetCurrentBorrowedBooks([FromQuery] BorrowingQueryParameter qp)
		{
			var result = await borrowingService.GetCurrentBorrowedBooksAsync(qp);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("overdue")]
		[Authorize(AuthenticationSchemes = "Bearer",Roles = "Admin")]
		public async Task<IActionResult> GetOverdueBooks([FromQuery] BorrowingQueryParameter qp)
		{
			var result = await borrowingService.GetOverdueBooksAsync(qp);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("most-borrowed")]
		public async Task<IActionResult> GetMostBorrowedBooks()
		{
			var result = await borrowingService.GetMostBorrowedBooksAsync();
			return StatusCode(result.StatusCode, result);
		}
	}
}

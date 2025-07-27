using Application.DTO_s.Author;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorController : ControllerBase
	{
		private readonly IAuthorService authorService;
		public AuthorController(IAuthorService authorService)
		{
			this.authorService = authorService;	
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll([FromQuery] string? searchTerm)
		{
			var result = await authorService.GetAllAuthorsAsync(searchTerm);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var result = await authorService.GetAuthorByIdAsync(id);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost("create")]
		public async Task<IActionResult> createAuthor(AuthorDto dto)
		{
			var result = await authorService.CreateAuthorAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateAuthor(AuthorDto dto)
		{
			var result = await authorService.UpdateAuthorAsync(dto);
			return StatusCode(result.StatusCode, result);
		}
	}
}

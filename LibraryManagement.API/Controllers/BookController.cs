using Application.DTO_s.Book;
using Application.Interfaces;
using Application.ResponseDTO_s.Book;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly IBookService bookService;
		public BookController(IBookService bookService)
		{
			this.bookService = bookService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateBook(CreateBookDto dto)
		{
			var result=await bookService.CreateBookAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> getBookById(int id)
		{
			var result = await bookService.GetBookByIdAsync(id);
			return StatusCode(result.StatusCode, result);
		}
		[HttpPut("update/{id:int}")]
		public async Task<IActionResult> updateBook(int id , UpdateBookDto dto)
		{	
			var result = await bookService.UpdateBookAsync(id,dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("author/{authorId}")]
		public async Task<IActionResult> getBooksByAuthor(int authorId)
		{
			var result = await bookService.GetBooksByAuthorAsync(authorId);
			return StatusCode(result.StatusCode, result);
		}
		[HttpGet("all")]
		public async Task<IActionResult> getAllBooks([FromQuery]BookQueryParameter queryParameter)
		{
			var result = await bookService.GetAllBooksAsync(queryParameter);
			return StatusCode(result.StatusCode, result);
		}
		
        [HttpGet("availableStatus")]
		public async Task<IActionResult> getAvailableBooks(bool isAvailable)
		{
			var result = await bookService.GetBooksAvailabilityAsync(isAvailable);
			return StatusCode(result.StatusCode, result);
		}
	}
}

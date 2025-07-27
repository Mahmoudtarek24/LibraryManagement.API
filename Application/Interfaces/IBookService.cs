using Application.DTO_s.Book;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IBookService
	{
		Task<ApiResponse<ConfirmationResponseDto>> CreateBookAsync(CreateBookDto dto);
		//Task<ApiResponse<List<AuthorResponseDto>>> GetAllAuthorsAsync(string searchTerm);
		Task<ApiResponse<BookResponseDto>> GetBookByIdAsync(int Id);
		Task<ApiResponse<ConfirmationResponseDto>> UpdateBookAsync(int id, UpdateBookDto dto);
		Task<ApiResponse<List<BookResponseDto>>> GetBooksByAuthorAsync(int authorId);

	}
}

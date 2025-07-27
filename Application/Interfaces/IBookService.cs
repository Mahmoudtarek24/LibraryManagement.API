using Application.DTO_s.Book;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Book;
using Domain.Models;
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
		Task<ApiResponse<BookResponseDto>> GetBookByIdAsync(int Id);
		Task<ApiResponse<ConfirmationResponseDto>> UpdateBookAsync(int id, UpdateBookDto dto);
		Task<ApiResponse<List<BooksByAuthorDto>>> GetBooksByAuthorAsync(int authorId);
		Task<PageResponse<List<BookWithAuthorDto>>> GetAllBooksAsync(BookQueryParameter queryParameter);
		Task<ApiResponse<List<BookWithAuthorDto>>> GetBooksAvailabilityAsync(bool isAvailable);

	}
}

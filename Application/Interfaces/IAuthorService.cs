using Application.DTO_s.Author;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IAuthorService
	{
		Task<ApiResponse<ConfirmationResponseDto>> CreateAuthorAsync(AuthorDto dto);
		Task<ApiResponse<List<AuthorResponseDto>>> GetAllAuthorsAsync(string searchTerm);
		Task<ApiResponse<AuthorResponseDto>> GetAuthorByIdAsync(int Id);
		Task<ApiResponse<ConfirmationResponseDto>> UpdateAuthorAsync(AuthorDto dto);

	}
}

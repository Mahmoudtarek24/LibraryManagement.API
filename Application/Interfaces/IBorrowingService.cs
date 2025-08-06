using Application.DTO_s.Borrowing;
using Application.ResponseDTO_s;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IBorrowingService
	{
		Task<ApiResponse<ConfirmationResponseDto>> AddBorrowingAsync(int userId, CreateBorrowingDto dto);
		Task<ApiResponse<ConfirmationResponseDto>> ReturnBorrowingAsync(int userId, ReturnBorrowingDto dto);
		Task<PageResponse<List<BorrowedBooks>>> GetCurrentBorrowedBooksAsync(BorrowingQueryParameter qp);
		Task<PageResponse<List<BorrowedBooks>>> GetOverdueBooksAsync(BorrowingQueryParameter qp);
		Task<ApiResponse<List<MemberBorrowingHistory>>> GetMemberBorrowingHistoryAsync(int memberId);
		Task<ApiResponse<List<MostBorrowedBooks>>> GetMostBorrowedBooksAsync();

	}
}

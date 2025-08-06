using Application.Constants.Enums;
using Application.DTO_s.Borrowing;
using Application.Interfaces;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Users;
using Domain.Entities;
using Domain.Interface;
using Domain.Models;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class BorrowingService : IBorrowingService
	{
		private readonly IUnitOfWork unitOfWork;
		public BorrowingService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;		
		}
		public async Task<ApiResponse<ConfirmationResponseDto>> AddBorrowingAsync(int userId,CreateBorrowingDto dto)
		{
			var IsAvailableBook =await unitOfWork.BookRepository.IsBookAvailableForRentalAsync(dto.BookId);
			if(!IsAvailableBook)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"book with id {dto.BookId} not found");

			var borrowing = new Borrowing()
			{
				BookId = dto.BookId,
				BorrowDate = dto.BorrowDate,
				ExpectedReturnDate = DateTime.Now.AddDays(5),
				MemberId = userId,
			};
				
			var result =await unitOfWork.BorrowingRepository.AddAsync(borrowing);

			if (result == -1)
				return ApiResponse<ConfirmationResponseDto>
					 .ValidationError($"The book with ID {dto.BookId} does not exist.");
			if (result == -2)
				return ApiResponse<ConfirmationResponseDto>
					  .Unauthorized($"The user with ID {userId} does not exist");
			if (result == -3)
				return ApiResponse<ConfirmationResponseDto>
					 .ValidationError($"User with ID {userId} has already borrowed the maximum number of books allowed (3)");
			if (result == -4)
				return ApiResponse<ConfirmationResponseDto>
					.ValidationError($"User with ID {userId} has already borrowed the book with ID {dto.BookId} ");

			var response=new ConfirmationResponseDto()
			{
				Message = $"User with ID {userId} successfully borrowed the book with ID {dto.BookId}.",
				Status =ConfirmationStatus.Borrowing,
			};	

			return ApiResponse<ConfirmationResponseDto>.Success(response);	
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> ReturnBorrowingAsync(int userId, ReturnBorrowingDto dto)
		{
			var returnBorrowing = new Borrowing()
			{
				BookId = dto.BookId,
				MemberId = userId,
				ActualReturnDate=DateTime.Now,
				IsReturned = true,
			};

			var result = await unitOfWork.BorrowingRepository.ReturnBookAsync(returnBorrowing);

			if (result == -1)
				return ApiResponse<ConfirmationResponseDto>
					   .NotFound($"No active borrowing found for user ID {userId} and book ID {dto.BookId}.");
			if (result == -2)
				return ApiResponse<ConfirmationResponseDto>
					 .Unauthorized($"The user with ID {userId} does not exist");
			
			var response = new ConfirmationResponseDto()
			{
				Message = $"Book with id {dto.BookId} has been returned successfully.",
				Status = ConfirmationStatus.Returned,
			};

			return ApiResponse<ConfirmationResponseDto>.Success(response);

		}
		public async Task<PageResponse<List<BorrowedBooks>>> GetCurrentBorrowedBooksAsync(BorrowingQueryParameter qp)
		{
			var filter = new BaseFilter()
			{
				SearchTearm = qp.SearchTearm,
				PageSize = qp.PageSize,
				PageNumber = qp.PageNumber,
			};

			var (Books,totalCount)=await unitOfWork.BorrowingRepository.GetCurrentBorrowedBooksAsync(filter);

			if (totalCount == 0)
			{
				var message = qp.SearchTearm is not null ? "No book found matching your criteria" :
					                                      "There are no borrowing records yet.";
				return PageResponse<List<BorrowedBooks>>.Create(null, 0, 0, 0, message);
			}
			return PageResponse<List<BorrowedBooks>>.Create(Books, qp.PageNumber, qp.PageSize, totalCount);
		}

		public async Task<PageResponse<List<BorrowedBooks>>> GetOverdueBooksAsync(BorrowingQueryParameter qp)
		{
			var filter = new BaseFilter()
			{
				SearchTearm = qp.SearchTearm,
				PageSize = qp.PageSize,
				PageNumber = qp.PageNumber,
			};

			var (Books, totalCount) = await unitOfWork.BorrowingRepository.GetOverdueBooksAsync(filter);

			if (totalCount == 0)
			{
				var message = qp.SearchTearm is not null ? "No Data found matching your criteria" :
														   "There are no Overdue Borrowing "; 
				return PageResponse<List<BorrowedBooks>>.Create(null, 0, 0, 0, message);
			}
			return PageResponse<List<BorrowedBooks>>.Create(Books, qp.PageNumber, qp.PageSize, totalCount);
		}

		public async Task<ApiResponse<List<MemberBorrowingHistory>>> GetMemberBorrowingHistoryAsync(int memberId)
		{
			var user = await unitOfWork.UserRepository.GetByIdAsync(memberId);
			if (user is null)
				return ApiResponse<List<MemberBorrowingHistory>>.NotFound("User not found");


			var BorrowingHistory = await unitOfWork.BorrowingRepository.GetMemberBorrowingHistoryAsync(memberId);

			if (BorrowingHistory.Count == 0)
				return ApiResponse<List<MemberBorrowingHistory>>
					.Success(null,200, "This user has not borrowed any books yet.");

			return ApiResponse<List<MemberBorrowingHistory>>.Success(BorrowingHistory);
		}

		public async Task<ApiResponse<List<MostBorrowedBooks>>> GetMostBorrowedBooksAsync()
		{
			var MostBorrowedBooks = await unitOfWork.BorrowingRepository.GetMostBorrowedBooksAsync();

			if (MostBorrowedBooks.Count == 0)
				return ApiResponse<List<MostBorrowedBooks>>
					.Success(null, 200, "There are no borrowing records yet.");

			return ApiResponse<List<MostBorrowedBooks>>.Success(MostBorrowedBooks);
		}
	}
}

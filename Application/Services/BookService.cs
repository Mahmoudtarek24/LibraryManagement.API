using Application.Constants.Enums;
using Application.DTO_s.Book;
using Application.Interfaces;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Book;
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
	public class BookService : IBookService
	{
		private readonly IUnitOfWork unitOfWork;
		public BookService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;		
		}
		public async Task<ApiResponse<ConfirmationResponseDto>> CreateBookAsync(CreateBookDto dto)
		{
			var author =await unitOfWork.AuthorRepository.GetByIdAsync(dto.AuthorId);
			if (author is null)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Author with Id {dto.AuthorId} not found");

			if (dto.PublishingDate > DateTime.Now)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Publishing date cannot be in the future");

			var book = new Book()
			{
				AuthorId = dto.AuthorId,
				Description = dto.Description,
				ImageUrl = dto.ImageUrl,
				IsAvailableForRental = dto.IsAvailableForRental,
				Name = dto.Name,
				PublishingDate = dto.PublishingDate
			};
			var result=await unitOfWork.BookRepository.AddAsync(book);		

			if(result==-1)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Author with Id {dto.AuthorId} not found");
			if(result==-2)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Publishing date cannot be in the future");

			var response =new ConfirmationResponseDto()
			{
				Message=$"Book with id {result} created Successfully",
				Status = ConfirmationStatus.Created
			};	
			return ApiResponse<ConfirmationResponseDto>.Success(response);	
		}

		public async Task<ApiResponse<BookResponseDto>> GetBookByIdAsync(int Id)
		{
			if (Id <= 0)
				return ApiResponse<BookResponseDto>.ValidationError("Not valid value for id");

			var book = await unitOfWork.BookRepository.GetByIdAsync(Id);

			var rseponse = new BookResponseDto()
			{
				Description = book.Description,
				PublishingDate = book.PublishingDate,
				AuthorId = book.AuthorId,
				ImageUrl = book.ImageUrl,
				Id = Id,
				IsAvailableForRental = book.IsAvailableForRental,
				Name = book.Name,
			};

			return ApiResponse<BookResponseDto>.Success(rseponse);
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> UpdateBookAsync(int id,UpdateBookDto dto)
		{
			if (dto.PublishingDate > DateTime.Now)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Publishing date cannot be in the future");

			var author = await unitOfWork.AuthorRepository.GetByIdAsync(dto.AuthorId ?? 0);
			if (author is null)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Author with Id {dto.AuthorId} not found");

			var book = await unitOfWork.BookRepository.GetByIdAsync(id);
			if(book is null)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Book with Id {id} Not found");


			if (!string.IsNullOrEmpty(dto.Name))
				book.Name = dto.Name;

			if (!string.IsNullOrEmpty(dto.Description))
				book.Description = dto.Description;

			if (!string.IsNullOrEmpty(dto.ImageUrl))
				book.ImageUrl = dto.ImageUrl;

			if (dto.IsAvailableForRental is not null)
				book.IsAvailableForRental = (bool)dto.IsAvailableForRental;

			if (dto.PublishingDate is not null)
				book.PublishingDate =(DateTime) dto.PublishingDate;

			if (dto.AuthorId is not null)
				book.AuthorId = (int)dto.AuthorId;

			var result = await unitOfWork.BookRepository.UpdateBook(book);

			if (result == -1)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Author with Id {dto.AuthorId} not found");
			if (result == -2)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Publishing date cannot be in the future");
			
			var response = new ConfirmationResponseDto()
			{
				Message = $"Book with id {result} Updated Successfully",
				Status = ConfirmationStatus.Updated,
			};
			return ApiResponse<ConfirmationResponseDto>.Success(response);
		}

		public async Task<ApiResponse<List<BooksByAuthorDto>>> GetBooksByAuthorAsync(int authorId)
		{
			if(authorId <= 0)
				return ApiResponse<List<BooksByAuthorDto>>.ValidationError("Not valid value for id");

			var Books = await unitOfWork.BookRepository.GetBooksByAuthorAsync(authorId);
		
			return ApiResponse<List<BooksByAuthorDto>>.Success(Books);		
		}

		public async Task<PageResponse<List<BookWithAuthorDto>>> GetAllBooksAsync(BookQueryParameter qP)
		{
			var filter = new BookFilter()
			{
				SearchTearm = qP.SearchTearm,
				IsAvailableForRental = qP.IsAvailableForRental,
				PageNumber = qP.PageNumber,
				PageSize = qP.PageSize,
			};

			var (books, TotalCount) = await unitOfWork.BookRepository.GetAllBookWithPagination(filter);

			if (TotalCount == 0)
				return PageResponse<List<BookWithAuthorDto>>.Create(null,0,0,0, "No books found matching your criteria");

			return PageResponse<List<BookWithAuthorDto>>.Create(books, qP.PageNumber, qP.PageSize, TotalCount);
		}

		public async Task<ApiResponse<List<BookWithAuthorDto>>> GetBooksAvailabilityAsync(bool isAvailable)
		{
			var books=await unitOfWork.BookRepository.GetBooksByAvailabilityAsync(isAvailable);

			return ApiResponse<List<BookWithAuthorDto>>.Success(books);
		}
	}
}

using Application.Constants.Enums;
using Application.DTO_s.Book;
using Application.Interfaces;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Book;
using Domain.Entities;
using Domain.Interface;
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
				book.IsAvailableForRental = dto.IsAvailableForRental;

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

		public async Task<ApiResponse<List<BookResponseDto>>> GetBooksByAuthorAsync(int authorId)
		{
			if(authorId <= 0)
				return ApiResponse<List<BookResponseDto>>.ValidationError("Not valid value for id");

			var Books = await unitOfWork.BookRepository.GetBooksByAuthorAsync(authorId);

			var response=Books.Select(e=>new BookResponseDto()
			{
				AuthorId = authorId,
				Description = e.Description,
				AuthorName=e.Author.Name,	
				ImageUrl=e.ImageUrl,
				IsAvailableForRental=e.IsAvailableForRental,	
				Id = e.Id,
				Name = e.Name,
				PublishingDate=e.PublishingDate,	
			}).ToList();		
			return ApiResponse<List<BookResponseDto>>.Success(response);		
		}
	}
}

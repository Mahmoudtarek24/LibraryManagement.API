using Application.Constants.Enums;
using Application.DTO_s.Author;
using Application.Interfaces;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Author;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class AuthorService : IAuthorService
	{
		private readonly IUnitOfWork unitOfWork;
		public AuthorService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;		
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> CreateAuthorAsync(AuthorDto dto)
		{
			var author = new Author
			{
				CreateOn = DateTime.Now,
				Name = dto.Name,
			};
			var result = await unitOfWork.AuthorRepository.AddAsync(author);

			if (result is null)
			{
				///throw data base error
			}
			var response = new ConfirmationResponseDto()
			{
				Message = $"Author With Id {result} created Successfully",
				Status = ConfirmationStatus.Created
			};
			return ApiResponse<ConfirmationResponseDto>.Success(response);
		}

		public async Task<ApiResponse<List<AuthorResponseDto>>> GetAllAuthorsAsync(string searchTerm)
		{
			var authors = await unitOfWork.AuthorRepository.GetAllEntities(searchTerm);

			if (authors.Count() == 0)
				return ApiResponse<List<AuthorResponseDto>>.Success(null, 200, "still no Author Created");

			var responseDto = authors.Select(e => new AuthorResponseDto
			{
				AuthorId = e.Id,
				AuthorName = e.Name,
				CreateOn = e.CreateOn,
			}).ToList();
			return ApiResponse<List<AuthorResponseDto>>.Success(responseDto);
		}

		public async Task<ApiResponse<AuthorResponseDto>> GetAuthorByIdAsync(int Id)
		{
			var author = await unitOfWork.AuthorRepository.GetByIdAsync(Id);

			if (author is null)
				return ApiResponse<AuthorResponseDto>.NotFound($"Not found Author With Id {Id}");

			var responseDto = new AuthorResponseDto
			{
				AuthorId = author.Id,
				AuthorName = author.Name,
				CreateOn = author.CreateOn,
			};
			return ApiResponse<AuthorResponseDto>.Success(responseDto);
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> UpdateAuthorAsync(AuthorDto dto)
		{
			var author = await unitOfWork.AuthorRepository.GetByIdAsync(dto.Id ?? 0);
			if (author is null)
				return ApiResponse<ConfirmationResponseDto>.NotFound($"Not found Author ");

			var updateAuthor = new Author()
			{
				Id = dto.Id ?? 0,
				Name = dto.Name,
			};

			var result = await unitOfWork.AuthorRepository.UpdateAsync(updateAuthor);

			var response = new ConfirmationResponseDto()
			{
				Message = $"Author with Id {dto.Id} update successfully",
				Status = ConfirmationStatus.Updated
			};
			return ApiResponse<ConfirmationResponseDto>.Success(response);
		}
	}
}

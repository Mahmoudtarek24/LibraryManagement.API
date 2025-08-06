using Application.Constants.Enums;
using Application.DTO_s.Users;
using Application.Interfaces;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Users;
using Domain.Entities;
using Domain.Interface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork unitOfWork;
	    private readonly ITokenGenerator tokenGenerator;	
		public UserService(IUnitOfWork unitOfWork,ITokenGenerator tokenGenerator)
		{
			this.unitOfWork = unitOfWork;	
			this.tokenGenerator = tokenGenerator;		
		}
		public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
		{
			if (id <= 0)
				return ApiResponse<UserResponseDto>.ValidationError("Not valid value for id");

			var user = await unitOfWork.UserRepository.GetByIdAsync(id);
			if (user is null)
				return ApiResponse<UserResponseDto>.NotFound("User not found");

			var response = MappUserToDto(user);
			return ApiResponse<UserResponseDto>.Success(response);
		}

		public async Task<ApiResponse<List<UserResponseDto>>> SearchUsersAsync(string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
				return ApiResponse<List<UserResponseDto>>.ValidationError("Search term cannot be empty");

			var users = await unitOfWork.UserRepository.SearchUsersAsync(searchTerm);
			if (users is null || users.Count is 0)
				return ApiResponse<List<UserResponseDto>>.Success(null,200, "No user found matching your criteria"); 

			var response = users.Select(e => MappUserToDto(e)).ToList();

			return ApiResponse<List<UserResponseDto>>.Success(response);
		}

		public async Task<PageResponse<List<UserResponseDto>>> GetAllUsersAsync(UserQueryParameter query)
		{
			var filter = new BaseFilter
			{
				SearchTearm = query.SearchTearm,
				PageNumber = query.PageNumber,
				PageSize = query.PageSize,
			};

			var (users, totalCount) = await unitOfWork.UserRepository.GetAllUsersAsync(filter);

			if (totalCount == 0)
				return PageResponse<List<UserResponseDto>>.Create(null,0,0,0,"No users found matching your criteria");

			var userDtos = users.Select(e=>new UserResponseDto
			{
				Id = e.Id,	
				CreateOn = e.CreateOn,
				Email = e.Email,
				FullName = e.FullName,
				Role = e.Role,
			}).ToList();

			return PageResponse<List<UserResponseDto>>.Create(userDtos, filter.PageNumber, filter.PageSize, totalCount);
		}

		public UserResponseDto MappUserToDto(User user)
		{
			return new UserResponseDto
			{
				CreateOn = user.CreateOn,
				Email = user.Email,
				FullName= user.FullName,
				Id = user.Id,	
				Role = user.Role,	
			};
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> ChangePasswordAsync(int userId, ChangePasswordDto dto)
		{
			if (userId <= 0 || string.IsNullOrWhiteSpace(dto.oldPassword) || string.IsNullOrWhiteSpace(dto.newPassword))
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Invalid input data.");

			var result = await unitOfWork.UserRepository.ChangePasswordAsync(dto.newPassword, dto.oldPassword, userId);

			if (result == -1)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Old password is incorrect.");

			var response = new ConfirmationResponseDto
			{
				Message = "Password changed successfully",
				Status= Constants.Enums.ConfirmationStatus.Updated,
			};
			return ApiResponse<ConfirmationResponseDto>.Success(response);
		}

		public async Task<ApiResponse<LoginRsponseDto>> LoginAsync(LoginDto dto)
		{
			var user = await unitOfWork.UserRepository.LoginAsync(dto.Email, dto.Password);

			if(user is null)
				return ApiResponse<LoginRsponseDto>.Unauthorized("Email or Password not Valid");

			var token = await tokenGenerator.GenerateAsync(user);

			var response = new LoginRsponseDto()
			{
				Token = token,
				ExpireDate = DateTime.Now.AddDays(1),
			};
			return ApiResponse<LoginRsponseDto>.Success(response,200,"Successfully login");
		}

		public async Task<ApiResponse<ConfirmationResponseDto>> CreateUserAsync(CreateUserDto dto)
		{
			if (await unitOfWork.UserRepository.IsEmailExistsAsync(dto.Email))
				return ApiResponse<ConfirmationResponseDto>.ValidationError("Email already exists");

			var user = new User()
			{
				Email = dto.Email,
				FullName = dto.FullName,
				Password = dto.Password,
				CreateOn = DateTime.Now,
				Role = AppRoles.Member.ToString(),
			};

			var result=await unitOfWork.UserRepository.AddAsync(user);
			if (result == -1)
				return ApiResponse<ConfirmationResponseDto>.ValidationError("");////المفروض في مشكله في role
			
			var response = new ConfirmationResponseDto()
			{
				Message = $"User with id {result} created Successfully",
				Status = ConfirmationStatus.Created
			};
			return ApiResponse<ConfirmationResponseDto>.Success(response);
		}
	}
}

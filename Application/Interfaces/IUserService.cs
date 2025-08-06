using Application.DTO_s.Users;
using Application.ResponseDTO_s;
using Application.ResponseDTO_s.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IUserService
	{
		Task<ApiResponse<ConfirmationResponseDto>> CreateUserAsync(CreateUserDto dto);
		Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id);
		Task<ApiResponse<List<UserResponseDto>>> SearchUsersAsync(string searchTerm);
		Task<PageResponse<List<UserResponseDto>>> GetAllUsersAsync(UserQueryParameter query);
		Task<ApiResponse<ConfirmationResponseDto>> ChangePasswordAsync(int userId,ChangePasswordDto dto);
		Task<ApiResponse<LoginRsponseDto>> LoginAsync(LoginDto dto);
	}
}

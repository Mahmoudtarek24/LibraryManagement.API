using Domain.Entities;
using Domain.Interface;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly LibraryDbContext context;
		public UserRepository(LibraryDbContext context)
		{
			this.context = context;
		}
		public async Task<int?> AddAsync(User entity)
		{
			var newUserId = new SqlParameter()
			{
				ParameterName = "@NewUserId",
				Direction = ParameterDirection.Output,
				SqlDbType = SqlDbType.Int,
			};
			
			await context.Database.ExecuteSqlInterpolatedAsync($@"EXEC spAddUser 
				@FullName={entity.FullName},
				@Email={entity.Email},
				@Password={entity.Password},
				@CreateOn={entity.CreateOn},
				@Role={entity.Role},
				@NewUserId={newUserId} OUTPUT");

			return (int)newUserId.Value;		
		}

		public void Delete(User entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}
		public async Task<User?> GetByIdAsync(int id) =>
			       (await context.Users.FromSqlInterpolated($"EXEC spGetUserById @UserId ={id}")
					     		     .ToListAsync()).FirstOrDefault();

		public async Task<List<User>> SearchUsersAsync(string searchTearm) =>
			     (await context.Users.FromSqlInterpolated($"EXEC spSearchMembers @SearchTearm ={searchTearm}")
							          .ToListAsync());


		public async Task<(List<User>, int)> GetAllUsersAsync(UserFilter filter)
		{
			var totalCount = new SqlParameter()
			{
				ParameterName = "@TotalCount",
				Direction = ParameterDirection.Output,
				SqlDbType = SqlDbType.Int,
				Value = 0
			};

			var users = await context.Users.FromSqlInterpolated($@"EXEC spGetAllMembers 
							@PageNumber={filter.PageNumber},
							@PageSize={filter.PageSize},
							@SearchTearm={filter.SearchTearm},
							@orderBy={filter.orderBy},
							@TotalCount={totalCount} OUTPUT ").ToListAsync();

			return (users, (int)totalCount.Value);
		}

		public async Task<int> ChangePasswordAsync(string newPassword, string oldPassword, int Id)
		{
			var result = new SqlParameter()
			{
				ParameterName = "@Result",
				Direction = ParameterDirection.Output,
				SqlDbType = SqlDbType.Int,
			};

			await context.Database.ExecuteSqlInterpolatedAsync($@"EXEC spChangePassword 
						@NewPassword={newPassword},
						@OldPassword={oldPassword},
						@UserId={Id},
						@Result={result} OUTPUT");

			return (int)result.Value;
		}

		public async Task<User?> LoginAsync(string email, string password)
		{
			var users = await context.Users
				.FromSqlInterpolated($@"EXEC spLogin  @Email={email},  @Password={password}").ToListAsync();

			return users.FirstOrDefault(); 
		}

		public async Task<bool> IsEmailExistsAsync(string email)
		{
			var existsParam = new SqlParameter()
			{
				ParameterName = "@Exists",
				Direction = ParameterDirection.Output,
				SqlDbType = SqlDbType.Bit
			};

			await context.Database.ExecuteSqlInterpolatedAsync(
				$"EXEC spCheckEmailExists @Email={email}, @Exists={existsParam} OUTPUT");

			return (bool)existsParam.Value;
		}
	}
}

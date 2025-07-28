using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
	public interface IUserRepository : IGenaricRepository<User>
	{
		Task<List<User>> SearchUsersAsync(string searchTearm);
		Task<(List<User>,int)> GetAllUsersAsync(UserFilter filter);
		Task<int> ChangePasswordAsync(string newPassword, string oldPassword, int Id);
		Task<User?> LoginAsync(string email, string password);
		Task<bool> IsEmailExistsAsync(string email);
	}
}

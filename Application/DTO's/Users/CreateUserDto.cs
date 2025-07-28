using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.Users
{
	public class CreateUserDto
	{
		public string FullName { get; set; } 
		public string Email { get; set; }
		public string Password { get; set; } 
	}
}

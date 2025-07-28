using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseDTO_s.Users
{
	public class UserResponseDto
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; } 
		public DateTime CreateOn { get; set; }
		public string Role { get; set; }
	}
}

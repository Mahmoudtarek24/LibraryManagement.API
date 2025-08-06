using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.Users
{
	public class ChangePasswordDto
	{
		public string oldPassword { get; set; }
		public string newPassword { get; set; }
	}
}

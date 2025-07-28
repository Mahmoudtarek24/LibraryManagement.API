using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseDTO_s.Users
{
	public class LoginRsponseDto
	{
		public string Token { get; set; }	
		public DateTime ExpireDate { get; set; }		
	}
}

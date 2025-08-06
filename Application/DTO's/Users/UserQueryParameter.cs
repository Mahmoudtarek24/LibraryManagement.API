using Application.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.Users
{
	public class UserQueryParameter
	{
		const int maxSize = 20;
		private int _pageSize;
		public string? SearchTearm { get; set; }
		public int PageNumber { get; set; } = 1;

		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value > maxSize ? maxSize : _pageSize; }	
		}
	}
}

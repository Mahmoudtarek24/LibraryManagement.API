using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseDTO_s
{
	public class PageResponse<T> : ApiResponse<T>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }

		public static PageResponse<T> Create(T data, int pageNumber, int pageSize, int totalRecords, string message = null)
		{
			return new PageResponse<T>()
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				Data = data,
				IsSuccessed = true,
				StatusCode = 200,
				Message = message,
				TotalPages = pageSize > 0 ? Math.Min((int)Math.Ceiling((double)totalRecords / pageSize), int.MaxValue) : 1
			};
		}
	}
}
//i can not used any method of parent class as return response becouse it method return  ApiResponse not pageresponse

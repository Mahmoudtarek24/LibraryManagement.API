using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseDTO_s
{
	public class ApiResponse<T> ///why can not make it static
	{
		public T Data { get; set; }
	    public int StatusCode { get; set; }		
		public bool IsSuccessed { get; set; }	
		public List<string> Erorrs { get; set; }
		public string Message { get; set; }
		public string ErrorCode {  get; set; }	

		public ApiResponse()
		{
			Erorrs=new List<string>();
			IsSuccessed = true;
		}

		public static ApiResponse<T> Success(T data, int statusCode = 200, string message = null)
		{
			return new ApiResponse<T>
			{
				Data = data,
				StatusCode = statusCode,
				Message = message
			};
		}
		public static ApiResponse<T> NotFound(string message)
		{
			return new ApiResponse<T>
			{
				StatusCode = 404,
				ErrorCode="NOT_FOUND",
				Message = message
			};
		}
		public static ApiResponse<T> ValidationError(string error)
		{
			return new ApiResponse<T>
			{
				StatusCode = 400,
				IsSuccessed = false,
				ErrorCode = "VALIDATION_ERROR",
				Erorrs = new List<string> { error }
			};
		}
		public static ApiResponse<T> ValidationError(List<string> errors)
		{
			var result = new ApiResponse<T>
			{
				StatusCode = 400,
				IsSuccessed = false,
				ErrorCode = "VALIDATION_ERROR",
			};
			result.Erorrs.AddRange(errors);
			return result;
		}
		public static ApiResponse<T> Unauthorized(string errors)
		{
			return new ApiResponse<T>
			{
				StatusCode = 401,
				IsSuccessed = false,
				ErrorCode = "UNAUTHORIZED",
				Message = errors,
			};

		}
	}
	
}

using Application.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.ResponseDTO_s
{
	public class ConfirmationResponseDto
	{
		public string Message { get; set; }
		[JsonPropertyName("result of the request")]
		public ConfirmationStatus Status { get; set; }
	}
}

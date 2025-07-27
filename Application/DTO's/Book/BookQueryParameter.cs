using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO_s.Book
{
	public class BookQueryParameter
	{
		[JsonPropertyName("Author/Book Name")]
		public string? SearchTearm { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public bool? IsAvailableForRental { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.Book
{
	public class UpdateBookDto
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool? IsAvailableForRental { get; set; }
		public string? ImageUrl { get; set; } = " ";
		public DateTime? PublishingDate { get; set; }
		public int? AuthorId { get; set; }
	}
}

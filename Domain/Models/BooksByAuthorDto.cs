using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class BooksByAuthorDto
	{
		public int Id { get; set; }
		public string BookName { get; set; }
		public string Description { get; set; }
		public bool? IsAvailableForRental { get; set; }
		public string ImageUrl { get; set; }
		public DateTime PublishingDate { get; set; }
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
	}
}

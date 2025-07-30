using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Book
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public bool IsAvailableForRental { get; set; }
		public string ImageUrl { get; set; } = null!;
		public DateTime PublishingDate { get; set; }
		public int AuthorId { get; set; }
		public virtual Author Author { get; set; } = null!;
		public virtual ICollection<Borrowing> Borrowings { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Borrowing
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		public int MemberId { get; set; }
		public bool? IsReturned { get; set; }
		public DateTime BorrowDate { get; set; }
		public DateTime ExpectedReturnDate { get; set; }
		public DateTime? ActualReturnDate { get; set; }
		public virtual Book Book { get; set; } = null!;
		public virtual User Member { get; set; } = null!;
	}
}

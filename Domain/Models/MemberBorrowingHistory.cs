using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class MemberBorrowingHistory
	{
		public int BorrowId { get; set; }
		public int MemberId { get; set; }
		public string FullName { get; set; }
		public int BookId { get; set; }
		public string Name { get; set; }
		public DateTime BorrowDate { get; set; }
		public DateTime ExpectedReturnDate { get; set; }
		public DateTime ActualReturnDate { get; set; }
	}
}

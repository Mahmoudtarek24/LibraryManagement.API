using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class MostBorrowedBooks
	{
		public int BookId { get; set; }
		public string Name { get; set; }
		public int BorrowedBooks { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s.Borrowing
{
	public class CreateBorrowingDto
	{
		public int BookId { get; set; }	
		public DateTime BorrowDate { get; set; }		
	}
}

using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
	public interface IBorrowingRepository :IGenaricRepository<Borrowing>
	{
		Task<int?> ReturnBookAsync(Borrowing entity);
		Task<int?> CountCurrentBorrows(int memberId);
		Task<(List<BorrowedBooks>,int)> GetCurrentBorrowedBooksAsync(BaseFilter filter);
		Task<(List<BorrowedBooks>,int)> GetOverdueBooksAsync(BaseFilter filter);
		Task<List<MemberBorrowingHistory>> GetMemberBorrowingHistoryAsync(int memberId);
		Task<List<MostBorrowedBooks>> GetMostBorrowedBooksAsync();
	}
}

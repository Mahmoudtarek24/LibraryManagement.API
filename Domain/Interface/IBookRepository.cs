using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
	public interface IBookRepository :IGenaricRepository<Book>
	{
		Task<int> UpdateBook(Book book);	
		Task<List<BooksByAuthorDto>> GetBooksByAuthorAsync(int AuthorId);
		Task<(List<BookWithAuthorDto>,int)> GetAllBookWithPagination(BookFilter filter);
		Task<List<BookWithAuthorDto>> GetBooksByAvailabilityAsync(bool isAvailable);
	}
}

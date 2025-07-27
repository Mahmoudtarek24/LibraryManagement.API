using Domain.Entities;
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
		Task<List<Book>> GetBooksByAuthorAsync(int AuthorId);	
	}
}

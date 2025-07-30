using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
	public interface IUnitOfWork
	{
		IAuthorRepository AuthorRepository { get; }
		IBookRepository BookRepository { get; }
		IUserRepository UserRepository { get; }
		IBorrowingRepository BorrowingRepository { get; }	
		Task BeginTransactionAsync();
		Task CommitTransaction();
		Task RollbackTransaction();
		Task SaveChangesAsync();
	}
}

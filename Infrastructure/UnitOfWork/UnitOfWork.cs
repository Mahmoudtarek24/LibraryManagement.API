using Domain.Interface;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
	internal class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly LibraryDbContext context;
		private IDbContextTransaction? objTrans = null;

		public IAuthorRepository AuthorRepository { get;private set; }

		public IBookRepository BookRepository { get; private set; }

		public UnitOfWork(LibraryDbContext context)
		{
			AuthorRepository = new AuthorRepository(context);
			BookRepository = new BookRepository(context);	
		}
		public async Task BeginTransactionAsync()
		{
			objTrans =await context.Database.BeginTransactionAsync();		
		}

		public async Task CommitTransaction()
		{
			try
			{
				await context.SaveChangesAsync();
				if(objTrans != null) 
					await objTrans.CommitAsync();	
			}
			catch
			{
				await RollbackTransaction();
				
			}
			finally
			{
				await DisposeTransaction();
			}
		}
		/// what will happend if we and empty constructor what effect her "	context?.Dispose();z"

		public void Dispose()
		{
			objTrans?.Dispose();
			context?.Dispose();
		}

		public async Task RollbackTransaction()
		{
			try
			{
				if (objTrans != null)
					await objTrans.RollbackAsync();
			}
			finally
			{
				await DisposeTransaction();
			}
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}
		private async Task DisposeTransaction()
		{
			if(objTrans != null)
			{
				await objTrans.DisposeAsync();	
				objTrans = null;	
			}
		
		}
	}

}

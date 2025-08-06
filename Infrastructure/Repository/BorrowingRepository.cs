using Domain.Entities;
using Domain.Interface;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	internal class BorrowingRepository : IBorrowingRepository
	{
		private readonly LibraryDbContext context;
		public BorrowingRepository(LibraryDbContext context)
		{
			this.context = context;
		}
		public async Task<int?> AddAsync(Borrowing entity)
		{
			var borrowId = new SqlParameter()
			{
				ParameterName = "@BorrowId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output
			};

			await context.Database.ExecuteSqlInterpolatedAsync($@"EXEC spBorrowBook
					@MemberId={entity.MemberId},
					@BookId={entity.BookId},
					@BorrowDate={entity.BorrowDate},
					@ExpectedReturnDate={entity.ExpectedReturnDate},
					@BorrowId={borrowId} output ");

			return (int)borrowId.Value;	
		}

		public void Delete(Borrowing entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Borrowing?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<int?> ReturnBookAsync(Borrowing entity)
		{
			var updateBorrowId = new SqlParameter()
			{
				ParameterName = "@updateBorrowId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output
			};
			var MemberId = new SqlParameter()
			{
				ParameterName = "@MemberId",
				Value = entity.MemberId,
				SqlDbType = SqlDbType.Int
			};
			var BookId = new SqlParameter()
			{
				ParameterName = "@BookId",
				Value = entity.BookId,
				SqlDbType = SqlDbType.Int
			};
			var ActualReturnDate = new SqlParameter()
			{
				ParameterName = "@ActualReturnDate",
				Value = entity.ActualReturnDate,
				SqlDbType = SqlDbType.DateTime
			};

			await context.Database.ExecuteSqlRawAsync(
				"EXEC spReturnBook @MemberId, @BookId, @ActualReturnDate, @updateBorrowId OUTPUT",
				MemberId, BookId, ActualReturnDate, updateBorrowId);

			return (int)updateBorrowId.Value;
		}

		public async Task<(List<BorrowedBooks>,int)> GetCurrentBorrowedBooksAsync(BaseFilter filter)
		{
			var totalCount = new SqlParameter()
			{
				ParameterName = "@totalCount",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output,
				Value = 0 //// why
			};

			var BorrowedBooks= await context.Database.SqlQuery<BorrowedBooks>($@"EXEC spGetCurrentBorrowedBooks
					@PageNumber = {filter.PageNumber},
					@PageSize = {filter.PageSize},
					@searchTearm = {filter.SearchTearm},
					@totalCount = {totalCount} output ").ToListAsync();

			return (BorrowedBooks,(int)totalCount.Value);
		}

		public async Task<(List<BorrowedBooks>, int)> GetOverdueBooksAsync(BaseFilter filter)
		{
			var totalCount = new SqlParameter()
			{
				ParameterName = "@totalCount",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output,
				Value = 0 //// why
			};

			var OverdueBooks = await context.Database.SqlQuery<BorrowedBooks>($@"EXEC spGetOverdueBooks
					@PageNumber = {filter.PageNumber},
					@PageSize = {filter.PageSize},
					@searchTearm = {filter.SearchTearm},
					@totalCount = {totalCount} output ").ToListAsync();

			return (OverdueBooks, (int)totalCount.Value);
		}

		public async Task<List<MemberBorrowingHistory>> GetMemberBorrowingHistoryAsync(int memberId)
		{
			var memberBorrowingHistory = await context.Database.SqlQuery<MemberBorrowingHistory>
				   ($@"EXEC spGetMemberBorrowingHistory
					@MemberId = {memberId} ").ToListAsync();

			return memberBorrowingHistory;
		}

		public async Task<List<MostBorrowedBooks>> GetMostBorrowedBooksAsync()
		{
			var mostBorrowedBooks = await context.Database.SqlQuery<MostBorrowedBooks>
				   ($@"EXEC spGetMostBorrowedBooks").ToListAsync();

			return mostBorrowedBooks;
		}

		public async Task<int?> CountCurrentBorrows(int memberId)
		{
			//var countCurrentBorrows = await context.Database.SqlQuery<int?>
			//	   ($@"EXEC spCountCurrentBorrows  @MemberId = {memberId} ").ToListAsync();

			//return countCurrentBorrows;
			return default(int?);	
		}
	}
}

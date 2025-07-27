using Domain.Entities;
using Domain.Interface;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	internal class BookRepository : IBookRepository
	{
		private readonly LibraryDbContext context; 
		public BookRepository(LibraryDbContext context)
		{
			this.context = context;
		} 
		public async Task<int?> AddAsync(Book entity)
		{
			var newBookId = new SqlParameter()
			{
				ParameterName = "@newBookId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output
			};

			await context.Database.ExecuteSqlInterpolatedAsync($@"EXEC spAddBook 
				@bookName = {entity.Name}, 
				@Description = {entity.Description}, 
				@ImageUrl = {entity.ImageUrl}, 
				@IsAvailableForRental = {entity.IsAvailableForRental}, 
				@PublishingDate = {entity.PublishingDate}, 
				@authorId = {entity.AuthorId},
				@newBookId = {newBookId} output"
			);
			
			return (int)newBookId.Value;
		}

		public void Delete(Book entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<Book?> GetByIdAsync(int id) =>
			   (await context.Books.FromSqlInterpolated($"EXEC spGetBookById @BookId={id}")
							 .ToListAsync()).FirstOrDefault();

		public async Task<int> UpdateBook(Book book)
		{
			var updateBookId = new SqlParameter()
			{
				ParameterName = "@updateBookId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.InputOutput,
				Value= book.Id	
			};

			await context.Database.ExecuteSqlInterpolatedAsync($@"EXEC spUpdateBook 
				@bookName = {book.Name}, 
				@Description = {book.Description}, 
				@ImageUrl = {book.ImageUrl}, 
				@IsAvailableForRental = {book.IsAvailableForRental}, 
				@PublishingDate = {book.PublishingDate}, 
				@authorId = {book.AuthorId},
				@updateBookId = {updateBookId} output"
			);

			return (int)updateBookId.Value;	
		}

		public async Task<List<Book>> GetBooksByAuthorAsync(int authorId)
		{
			var Books= await context.Books
				.FromSqlInterpolated($"EXEC  spGetBooksByAuthor @authorId={authorId}").ToListAsync();
			return Books;	
		}

	}
}

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
	internal class AuthorRepository :IAuthorRepository
	{
		protected readonly LibraryDbContext context;
		public AuthorRepository(LibraryDbContext context) 
		{
			this.context = context;		
		}

		public async Task<int?> AddAsync(Author author)
		{
			var newAuthorId = new SqlParameter()
			{
				ParameterName = "@NewAuthorId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output,
			};

			int affectedRows = await context.Database.ExecuteSqlInterpolatedAsync(
			$"EXEC spCreateAuthor @AuthorName={author.Name} ,@createOn={author.CreateOn} ,@NewAuthorId={newAuthorId} output");

			if (affectedRows == 0 || newAuthorId.Value is DBNull)
				return null;

			return (int)newAuthorId.Value!;
		}

		public void Delete(Author entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Author>> GetAllEntities(string searchTerm = null) =>
				await context.Authors.FromSqlInterpolated($"EXEC spGetAllAuthor @SearchTerm={searchTerm}")
						   .ToListAsync();

		public async Task<Author?> GetByIdAsync(int id) =>
				(await context.Authors.FromSqlInterpolated($"EXEC spGetAuthorById @AuthorId={id}")
							 .ToListAsync()).FirstOrDefault();

		public async Task<bool> UpdateAsync(Author author)
		{
			var AuthorId = new SqlParameter
			{
				ParameterName = "@NewAuthorId",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.InputOutput,
				Value = author.Id
			};

			await context.Database.ExecuteSqlInterpolatedAsync(
				$"EXEC spUpdateAuthor @AuthorName={author.Name}, @NewAuthorId={AuthorId} output");

			return (int)AuthorId.Value == author.Id;
		}
	}
}

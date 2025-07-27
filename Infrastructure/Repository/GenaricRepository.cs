using Domain.Interface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	internal class GenaricRepository<T> //: IGenaricRepository<T> where T : class
	{
		//protected readonly LibraryDbContext context;
		//private readonly DbSet<T> entity;

		//public GenaricRepository(LibraryDbContext _context)
		//{
		//	this.context = _context;
		//	this.entity = context.Set<T>();
		//}
		////public async Task AddRangeAsync(IEnumerable<T> entities)
		////{
		////	await context.BulkInsertAsync(entities.ToList());
		////}
		//public async Task AddAsync(T entity)
		//{
		//	await this.entity.AddAsync(entity);
		//}
		//public void Delete(T entity)
		//{
		//	this.entity.Remove(entity);
		//}
		//public async Task DeleteByIdAsync(int id)
		//{
		//	var item = await entity.FindAsync(id);
		//	if (item != null)
		//	{
		//		entity.Remove(item);
		//	}
		//}
		////public virtual async Task<IEnumerable<T>> GetAllEntities()
		////{
		////}
		//public virtual async Task<T?> GetByIdAsync(int id)
		//{
		//	return await entity.FindAsync(id);
		//}
	}
}

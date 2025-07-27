using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interface
{
	public interface IAuthorRepository :IGenaricRepository<Author>
	{
		Task<IEnumerable<Author>> GetAllEntities(string searchtearm);
		Task<bool> UpdateAsync(Author author);
	}
}

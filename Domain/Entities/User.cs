using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public DateTime CreateOn { get; set; }
		public string Role { get; set; } = null!;
		public virtual ICollection<Borrowing> Borrowings { get; set; }

	}
}

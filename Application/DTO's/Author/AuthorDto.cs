using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO_s.Author
{
	public class AuthorDto
	{
		[JsonPropertyName("Id :'Requird in update'")]
		public int? Id { get; set; }	
		public string Name { get; set; }	
	}
}

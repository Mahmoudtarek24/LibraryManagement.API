using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateOn { get; set; }
        public ICollection<Book> Books { get; set; }    
    }
}
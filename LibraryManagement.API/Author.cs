using System;
using System.Collections.Generic;

namespace LibraryManagement;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateOn { get; set; }
}

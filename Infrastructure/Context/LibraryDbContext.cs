using Domain.Entities;
using Infrastructure;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext() { }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
		OnModelCreatingPartial(modelBuilder);
    }
    public virtual DbSet<Author> Authors { get; set; }
	public virtual DbSet<Book> Books { get; set; }
	public virtual DbSet<User> Users { get; set; }
}

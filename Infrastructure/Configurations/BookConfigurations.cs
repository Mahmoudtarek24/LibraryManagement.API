using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Configurations
{
	internal class BookConfigurations : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> entity)
		{
			entity.HasKey(e => e.Id).HasName("PK__Book__3214EC07F85AA831");

			entity.ToTable("Book");

			entity.Property(e => e.AuthorId).HasColumnName("authorId");
			entity.Property(e => e.Description)
				.HasMaxLength(300)
				.IsUnicode(false);
			entity.Property(e => e.ImageUrl)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsAvailableForRental).HasDefaultValue(true);
			entity.Property(e => e.Name)
				.HasMaxLength(60)
				.IsUnicode(false);
			entity.Property(e => e.PublishingDate).HasColumnType("datetime");

			entity.HasOne(d => d.Author).WithMany(p => p.Books)
				.HasForeignKey(d => d.AuthorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_BOOK_Author");
		}
	}
}

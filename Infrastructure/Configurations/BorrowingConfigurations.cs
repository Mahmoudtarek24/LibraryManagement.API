using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Configurations
{
	internal class BorrowingConfigurations : IEntityTypeConfiguration<Borrowing>
	{
		public void Configure(EntityTypeBuilder<Borrowing> entity)
		{
			entity.HasKey(e => e.Id).HasName("PK__Borrowin__3214EC07396D08FB");
			entity.ToTable("Borrowing");
			entity.Property(e => e.ActualReturnDate).HasColumnType("datetime");
			entity.Property(e => e.BorrowDate).HasColumnType("datetime");
			entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");
			entity.Property(e => e.IsReturned).HasDefaultValue(false);
			entity.HasOne(d => d.Book).WithMany(p => p.Borrowings)
				.HasForeignKey(d => d.BookId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Borrowing_Book");
			
			entity.HasOne(d => d.Member).WithMany(p => p.Borrowings)
				.HasForeignKey(d => d.MemberId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Borrowing_Users");
		}
	}
}

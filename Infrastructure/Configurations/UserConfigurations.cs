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
	internal class UserConfigurations : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> entity)
		{
			entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07022FE202");
			entity.HasIndex(e => e.Email, "UQ__Users__A9D1053422396879").IsUnique();
			entity.Property(e => e.CreateOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
			entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
			entity.Property(e => e.FullName).HasMaxLength(35).IsUnicode(false);
			entity.Property(e => e.Password).HasMaxLength(30).IsUnicode(false);
			entity.Property(e => e.Role).HasMaxLength(20).IsUnicode(false);
		}
	}
}

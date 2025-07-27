using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
	public class AuthorConfigurations : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> entity)
		{
			entity.HasKey(e => e.Id).HasName("PK__Author__3214EC07E8094359");
			entity.ToTable("Author");
			entity.Property(e => e.CreateOn).HasColumnType("datetime");
			entity.Property(e => e.Name).HasMaxLength(50);
		}
	}
}

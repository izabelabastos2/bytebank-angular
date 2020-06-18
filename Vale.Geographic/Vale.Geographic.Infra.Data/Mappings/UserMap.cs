using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Entities.Authorization;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.CreatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.LastUpdatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(250)").HasMaxLength(250);
            builder.Property(p => p.Email).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.Matricula).HasColumnType("varchar(15)").HasMaxLength(15).IsRequired();
            builder.Property(p => p.Profile).IsRequired();

        }

    }
}

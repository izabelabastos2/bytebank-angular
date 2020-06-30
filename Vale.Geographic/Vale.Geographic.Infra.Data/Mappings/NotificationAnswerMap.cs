using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class NotificationAnswerMap : IEntityTypeConfiguration<NotificationAnswer>
    {
        public void Configure(EntityTypeBuilder<NotificationAnswer> builder)
        {
            builder.ToTable("NotificationAnswers");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FocalPointId).IsRequired();
            builder.Property(p => p.NotificationId).IsRequired();
            builder.Property(p => p.Answered).HasColumnType("bit");
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.CreatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.LastUpdatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);

        }
    }
}

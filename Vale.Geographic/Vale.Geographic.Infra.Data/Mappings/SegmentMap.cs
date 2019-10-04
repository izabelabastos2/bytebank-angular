using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vale.Geographic.Domain.Entities;
using System.Linq;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class SegmentMap : IEntityTypeConfiguration<Segment>
    {
        public void Configure(EntityTypeBuilder<Segment> builder)
        {
            builder.ToTable("Segment");
            // builder.Property(p => p.Id).HasColumnType("int").HasValueGenerator<IntKey>();;

            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");

            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.Description).HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.Location).HasColumnType("geography");
            builder.Property(p => p.Length).IsRequired().HasColumnType("decimal(5,2)");
        }
    }
}
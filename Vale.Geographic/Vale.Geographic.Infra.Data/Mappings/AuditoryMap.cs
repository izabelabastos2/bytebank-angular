using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class AuditoryMap : IEntityTypeConfiguration<Auditory>
    {
        public void Configure(EntityTypeBuilder<Auditory> builder)
        {
            builder.ToTable("Auditory");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.CreatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.LastUpdatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.TypeEntitie).IsRequired();
            builder.Property(p => p.OldValue).IsRequired().HasColumnType("varchar(MAX)");
            builder.Property(p => p.NewValue).IsRequired().HasColumnType("varchar(MAX)");

            builder.HasOne<Area>(p => p.Area)
                .WithMany()
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<PointOfInterest>(p => p.PointOfInterest)
                .WithMany()
                .HasForeignKey(x => x.PointOfInterestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Category>(p => p.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

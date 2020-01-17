using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vale.Geographic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class PointOfInterestMap: IEntityTypeConfiguration<PointOfInterest>
    {
        public void Configure(EntityTypeBuilder<PointOfInterest> builder)
        {
            builder.ToTable("PointOfInterest");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(150)").HasMaxLength(150);
            builder.Property(p => p.Description).HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.Icon).HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.Location).IsRequired().HasColumnType("geography");


            builder.HasOne<Category>(p => p.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Area>(p => p.Area)
                .WithMany()
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
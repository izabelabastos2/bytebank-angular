using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vale.Geographic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Vale.Geographic.Infra.Data.Mappings
{
    public class SitesPerimetersMap : IEntityTypeConfiguration<SitesPerimeter>
    {
        public void Configure(EntityTypeBuilder<SitesPerimeter> builder)
        {
            builder.ToTable("SitesPerimeters");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.CreatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.LastUpdatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);

            builder.HasOne<Area>(p => p.Area)
                .WithMany()
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Site>(p => p.Site)
                .WithMany()
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vale.Geographic.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Vale.Geographic.Infra.Data.Mappings
{
    public class FocalPointMap : IEntityTypeConfiguration<FocalPoint>
    {
        public void Configure(EntityTypeBuilder<FocalPoint> builder)
        {
            builder.ToTable("FocalPoints");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.CreatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.LastUpdatedBy).IsRequired().HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(250)").HasMaxLength(250);
            builder.Property(p => p.Matricula).IsRequired().HasColumnType("varchar(15)").HasMaxLength(15);
            builder.Property(p => p.PhoneNumber).HasMaxLength(50);

            builder.HasOne<Area>(p => p.Locality)
                .WithMany()
                .HasForeignKey(x => x.LocalityId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            builder.HasOne<PointOfInterest>(p => p.PointOfInterest)
                .WithMany()
                .HasForeignKey(x => x.PointOfInterestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}

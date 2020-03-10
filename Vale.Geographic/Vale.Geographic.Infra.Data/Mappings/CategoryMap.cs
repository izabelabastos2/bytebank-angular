using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vale.Geographic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Vale.Geographic.Infra.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categorys");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.LastUpdatedAt).IsRequired().HasColumnType("datetime");
            builder.Property(p => p.Status).IsRequired().HasColumnType("bit");
            builder.Property(p => p.CreatedBy).HasColumnType("varchar(100)");
            builder.Property(p => p.LastUpdatedBy).HasColumnType("varchar(100)");
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.TypeEntitie).IsRequired();

        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Vale.Geographic.Domain.Entities;

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

            builder.Property(p => p.Description).HasColumnType("varchar(255)").HasMaxLength(255);
            builder.Property(p => p.TypeEntitie).IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialManager.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        // Configuração do Auto-relacionamento
        builder.HasOne(c => c.ParentCategory)
               .WithMany(c => c.SubCategories)
               .HasForeignKey(c => c.ParentCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
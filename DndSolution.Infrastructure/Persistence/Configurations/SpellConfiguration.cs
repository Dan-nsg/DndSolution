using DndSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DndSolution.Infrastructure.Persistence.Configurations
{
    public class SpellConfiguration : IEntityTypeConfiguration<Spell>
    {
        public void Configure(EntityTypeBuilder<Spell> builder)
        {
            builder.ToTable("Spells");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Index)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .IsRequired();

            builder.Property(s => s.Components)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(s => s.Index)
                .IsUnique();

            builder.HasIndex(s => s.Name);
        }
    }
}
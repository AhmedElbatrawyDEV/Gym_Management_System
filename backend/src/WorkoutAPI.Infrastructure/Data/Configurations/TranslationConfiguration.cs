using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.ToTable("Translations");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.EntityType).HasMaxLength(50).IsRequired();
        builder.Property(t => t.EntityId).IsRequired();
        builder.Property(t => t.Culture).HasMaxLength(10).IsRequired().HasDefaultValue("en");
        builder.Property(t => t.Field).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Value).HasMaxLength(2000).IsRequired();

        builder.HasIndex(t => new { t.EntityType, t.EntityId, t.Culture, t.Field }).IsUnique();
        builder.HasIndex(t => t.EntityType);
        builder.HasIndex(t => t.Culture);
    }
}

// =====
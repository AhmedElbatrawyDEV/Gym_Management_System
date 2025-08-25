using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.UserId).IsRequired();
        builder.Property(m => m.MembershipStartDate).IsRequired();
        builder.Property(m => m.MembershipEndDate).IsRequired();
        builder.Property(m => m.MembershipType).IsRequired();
        builder.Property(m => m.IsActiveMember).IsRequired().HasDefaultValue(true);

        builder.HasIndex(m => m.UserId).IsUnique();
        builder.HasIndex(m => m.MembershipType);
        builder.HasIndex(m => m.IsActiveMember);
        builder.HasIndex(m => m.MembershipEndDate);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====
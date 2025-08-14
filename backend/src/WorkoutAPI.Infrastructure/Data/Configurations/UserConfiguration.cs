using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);
            
        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);
            
        builder.Property(u => u.CreatedBy)
            .HasMaxLength(255);
            
        builder.Property(u => u.UpdatedBy)
            .HasMaxLength(255);
            
        // Relationships
        builder.HasMany(u => u.WorkoutSessions)
            .WithOne(ws => ws.User)
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(u => u.UserWorkoutPlans)
            .WithOne(uwp => uwp.User)
            .HasForeignKey(uwp => uwp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Specialization)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(t => t.Certification)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(t => t.HourlyRate)
            .HasPrecision(18, 2);
            
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255);
            
        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(255);
            
        // Relationships
        builder.HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<Trainer>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(t => t.ScheduledSessions)
            .WithOne(ws => ws.Trainer)
            .HasForeignKey(ws => ws.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}


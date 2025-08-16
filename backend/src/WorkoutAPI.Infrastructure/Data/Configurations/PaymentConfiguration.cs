using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);
            
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(255);
            
        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(255);
            
        // Relationships
        builder.HasOne(p => p.Member)
            .WithMany()
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


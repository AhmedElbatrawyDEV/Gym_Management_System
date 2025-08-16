using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentials>
{
    public void Configure(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.ToTable("UserCredentials");
        
        builder.HasKey(uc => uc.Id);
        
        builder.Property(uc => uc.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(uc => uc.Salt)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(uc => uc.CreatedBy)
            .HasMaxLength(255);
            
        builder.Property(uc => uc.UpdatedBy)
            .HasMaxLength(255);
            
        // Relationships
        builder.HasOne(uc => uc.User)
            .WithOne()
            .HasForeignKey<UserCredentials>(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


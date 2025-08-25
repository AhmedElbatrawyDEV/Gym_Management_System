using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.UserId).IsRequired();
        builder.Property(i => i.PaymentId).IsRequired();
        builder.Property(i => i.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(i => i.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.TaxAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.Status).IsRequired();
        builder.Property(i => i.PaidAt);

        builder.HasIndex(i => i.InvoiceNumber).IsUnique();
        builder.HasIndex(i => i.UserId);
        builder.HasIndex(i => i.PaymentId);
        builder.HasIndex(i => i.Status);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Payment>()
            .WithOne()
            .HasForeignKey<Invoice>(i => i.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====
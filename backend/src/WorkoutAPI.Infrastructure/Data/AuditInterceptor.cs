using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Infrastructure.Interfaces;

namespace WorkoutAPI.Infrastructure.Data;

public class AuditInterceptor : SaveChangesInterceptor {
    private readonly ICurrentUserService _currentUserService;

    public AuditInterceptor(ICurrentUserService currentUserService) {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result) {
        ApplyAuditInfo(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default) {
        ApplyAuditInfo(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditInfo(DbContext? context) {
        if (context == null) return;

        var currentUser = _currentUserService.UserId;
        var currentTime = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreated(currentUser);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdated(currentUser);
            }
            else if (entry.State == EntityState.Deleted &&
                     entry.Entity is ISoftDelete softDeleteEntity)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.SoftDelete(currentUser);
            }
        }
    }
}

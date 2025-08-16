namespace WorkoutAPI.Infrastructure.Interfaces;

public interface ISoftDelete
{
    void SoftDelete(string? deletedBy = null);
}

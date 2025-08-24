namespace WorkoutAPI.Infrastructure.Interfaces;

public interface ICurrentUserService {
    string? UserId { get; }
    string? Username { get; }
    bool IsAuthenticated { get; }
}

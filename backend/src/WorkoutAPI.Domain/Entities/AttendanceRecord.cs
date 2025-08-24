using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;


namespace WorkoutAPI.Domain.Entities {
    public class AttendanceRecord : BaseEntity {
        public Guid UserId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? DurationMinutes { get; set; }
        public ActivityType ActivityType { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public void CheckOut() {
            if (CheckOutTime.HasValue)
                throw new InvalidOperationException("User has already checked out");

            CheckOutTime = DateTime.UtcNow;
            DurationMinutes = (int)(CheckOutTime.Value - CheckInTime).TotalMinutes;
        }
    }
}
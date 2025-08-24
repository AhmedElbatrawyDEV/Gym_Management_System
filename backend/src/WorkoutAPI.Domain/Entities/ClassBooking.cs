using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;


namespace WorkoutAPI.Domain.Entities {

    public class ClassBooking : BaseEntity {
        public Guid UserId { get; set; }
        public Guid ClassScheduleId { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ClassSchedule ClassSchedule { get; set; } = null!;
    }
}
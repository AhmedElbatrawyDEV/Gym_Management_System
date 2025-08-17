using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Entities;


    public class ClassSchedule : BaseEntity
    {
        public Guid GymClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public GymClass GymClass { get; set; } = null!;
        public ICollection<ClassBooking> Bookings { get; set; } = new List<ClassBooking>();

        public int CurrentBookings => Bookings.Count(b => b.Status == BookingStatus.Confirmed);
        public bool IsFull => CurrentBookings >= GymClass.Capacity;
        public bool CanBook => IsActive && !IsFull && StartTime > DateTime.UtcNow;
    }

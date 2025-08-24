// Services/IAdminService.cs
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Services {


    public interface IAttendanceService {
        Task<AttendanceRecordResponse> CheckInAsync(Guid userId, ActivityType activityType);
        Task<AttendanceRecordResponse> CheckOutAsync(Guid recordId);
        Task<IEnumerable<AttendanceRecordResponse>> GetUserAttendanceAsync(Guid userId, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<GymClassResponse>> GetGymClassesAsync();
        Task<GymClassResponse> CreateGymClassAsync(CreateGymClassRequest request);
        Task<ClassBookingResponse> BookClassAsync(Guid userId, Guid scheduleId);
        Task CancelBookingAsync(Guid bookingId);
    }
    public class AttendanceService : IAttendanceService {
        private readonly WorkoutDbContext _context;

        public AttendanceService(WorkoutDbContext context) {
            _context = context;
        }

        public async Task<AttendanceRecordResponse> CheckInAsync(Guid userId, ActivityType activityType) {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Check if user is already checked in
            var existingRecord = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.UserId == userId && a.CheckOutTime == null);

            if (existingRecord != null)
                throw new InvalidOperationException("User is already checked in");

            var record = new AttendanceRecord {
                Id = Guid.NewGuid(),
                UserId = userId,
                CheckInTime = DateTime.UtcNow,
                ActivityType = activityType,
                CreatedAt = DateTime.UtcNow
            };

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();

            return MapAttendanceToResponse(record);
        }

        public async Task<AttendanceRecordResponse> CheckOutAsync(Guid recordId) {
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.Id == recordId);

            if (record == null)
                throw new KeyNotFoundException("Attendance record not found");

            record.CheckOut();
            record.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapAttendanceToResponse(record);
        }

        public async Task<IEnumerable<AttendanceRecordResponse>> GetUserAttendanceAsync(Guid userId, DateTime? fromDate, DateTime? toDate) {
            var query = _context.AttendanceRecords
                .Where(a => a.UserId == userId);

            if (fromDate.HasValue)
                query = query.Where(a => a.CheckInTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(a => a.CheckInTime <= toDate.Value);

            var records = await query
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync();

            return records.Select(MapAttendanceToResponse);
        }

        public async Task<IEnumerable<GymClassResponse>> GetGymClassesAsync() {
            var classes = await _context.GymClasses
                .Include(c => c.Instructor)
                .Include(c => c.Schedules)
                .ThenInclude(s => s.Bookings)
                .Where(c => c.IsActive)
                .ToListAsync();

            return classes.Select(MapClassToResponse);
        }

        public async Task<GymClassResponse> CreateGymClassAsync(CreateGymClassRequest request) {
            var gymClass = new GymClass {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                InstructorId = request.InstructorId,
                MaxCapacity = request.MaxCapacity,
                Duration = request.Duration,
                Difficulty = request.Difficulty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.GymClasses.Add(gymClass);
            await _context.SaveChangesAsync();

            // Load with instructor details
            gymClass = await _context.GymClasses
                .Include(c => c.Instructor)
                .FirstAsync(c => c.Id == gymClass.Id);

            return MapClassToResponse(gymClass);
        }

        public async Task<ClassBookingResponse> BookClassAsync(Guid userId, Guid scheduleId) {
            var schedule = await _context.ClassSchedules
                .Include(s => s.GymClass)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null)
                throw new KeyNotFoundException("Class schedule not found");

            if (!schedule.CanBook)
                throw new InvalidOperationException("Cannot book this class");

            // Check if user already booked
            var existingBooking = await _context.ClassBookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ClassScheduleId == scheduleId);

            if (existingBooking != null)
                throw new InvalidOperationException("User already booked this class");

            var booking = new ClassBooking {
                Id = Guid.NewGuid(),
                UserId = userId,
                ClassScheduleId = scheduleId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            _context.ClassBookings.Add(booking);
            await _context.SaveChangesAsync();

            return new ClassBookingResponse(
                Id: booking.Id,
                UserId: booking.UserId,
                ClassScheduleId: booking.ClassScheduleId,
                BookingDate: booking.BookingDate,
                Status: booking.Status
            );
        }

        public async Task CancelBookingAsync(Guid bookingId) {
            var booking = await _context.ClassBookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private static AttendanceRecordResponse MapAttendanceToResponse(AttendanceRecord record) {
            return new AttendanceRecordResponse(
                Id: record.Id,
                UserId: record.UserId,
                CheckInTime: record.CheckInTime,
                CheckOutTime: record.CheckOutTime,
                DurationMinutes: record.DurationMinutes,
                ActivityType: record.ActivityType
            );
        }

        private static GymClassResponse MapClassToResponse(GymClass gymClass) {
            return new GymClassResponse(
                Id: gymClass.Id,
                Name: gymClass.Name,
                Description: gymClass.Description,
                InstructorId: gymClass.InstructorId,
                InstructorName: gymClass.Instructor?.FirstName + " " + gymClass.Instructor?.LastName,
                MaxCapacity: gymClass.MaxCapacity,
                CurrentBookings: gymClass.Schedules.SelectMany(s => s.Bookings).Count(b => b.Status == BookingStatus.Confirmed),
                Duration: gymClass.Duration,
                Difficulty: gymClass.Difficulty,
                IsActive: gymClass.IsActive,
                CreatedAt: gymClass.CreatedAt
            );
        }
    }
}



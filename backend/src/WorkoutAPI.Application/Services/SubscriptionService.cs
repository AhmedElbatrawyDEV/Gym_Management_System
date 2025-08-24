// Services/IAdminService.cs
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Services {
    public interface ISubscriptionService {
        Task<IEnumerable<SubscriptionPlanResponse>> GetAllPlansAsync();
        Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request);
        Task<SubscriptionPlanResponse> UpdatePlanAsync(Guid id, UpdateSubscriptionPlanRequest request);
        Task<UserSubscriptionResponse> AssignSubscriptionAsync(Guid userId, AssignSubscriptionRequest request);
        Task<IEnumerable<UserSubscriptionResponse>> GetUserSubscriptionsAsync(Guid userId);
        Task<UserSubscriptionResponse> ExtendSubscriptionAsync(Guid subscriptionId, int extensionDays);
        Task CancelSubscriptionAsync(Guid subscriptionId);
    }
    // SubscriptionService.cs
    public class SubscriptionService : ISubscriptionService {
        private readonly WorkoutDbContext _context;

        public SubscriptionService(WorkoutDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<SubscriptionPlanResponse>> GetAllPlansAsync() {
            var plans = await _context.SubscriptionPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Price)
                .ToListAsync();

            return plans.Select(MapPlanToResponse);
        }

        public async Task<SubscriptionPlanResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request) {
            var plan = new SubscriptionPlan {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DurationDays = request.DurationDays,
                Features = request.Features,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();

            return MapPlanToResponse(plan);
        }

        public async Task<SubscriptionPlanResponse> UpdatePlanAsync(Guid id, UpdateSubscriptionPlanRequest request) {
            var plan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                throw new KeyNotFoundException("Subscription plan not found");

            plan.Name = request.Name;
            plan.Description = request.Description;
            plan.Price = request.Price;
            plan.DurationDays = request.DurationDays;
            plan.Features = request.Features;
            plan.IsActive = request.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapPlanToResponse(plan);
        }

        public async Task<UserSubscriptionResponse> AssignSubscriptionAsync(Guid userId, AssignSubscriptionRequest request) {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var plan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == request.SubscriptionPlanId);

            if (plan == null)
                throw new KeyNotFoundException("Subscription plan not found");

            var startDate = request.StartDate ?? DateTime.UtcNow;
            var endDate = startDate.AddDays(plan.DurationDays);

            var subscription = new UserSubscription {
                Id = Guid.NewGuid(),
                UserId = userId,
                SubscriptionPlanId = request.SubscriptionPlanId,
                StartDate = startDate,
                EndDate = endDate,
                Status = SubscriptionStatus.Active,
                AutoRenew = request.AutoRenew,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            // Load the subscription with plan details
            subscription = await _context.UserSubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstAsync(s => s.Id == subscription.Id);

            return MapSubscriptionToResponse(subscription);
        }

        public async Task<IEnumerable<UserSubscriptionResponse>> GetUserSubscriptionsAsync(Guid userId) {
            var subscriptions = await _context.UserSubscriptions
                .Include(s => s.SubscriptionPlan)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return subscriptions.Select(MapSubscriptionToResponse);
        }

        public async Task<UserSubscriptionResponse> ExtendSubscriptionAsync(Guid subscriptionId, int extensionDays) {
            var subscription = await _context.UserSubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null)
                throw new KeyNotFoundException("Subscription not found");

            subscription.EndDate = subscription.EndDate.AddDays(extensionDays);
            subscription.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapSubscriptionToResponse(subscription);
        }

        public async Task CancelSubscriptionAsync(Guid subscriptionId) {
            var subscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null)
                throw new KeyNotFoundException("Subscription not found");

            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.AutoRenew = false;
            subscription.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private static SubscriptionPlanResponse MapPlanToResponse(SubscriptionPlan plan) {
            return new SubscriptionPlanResponse(
                Id: plan.Id,
                Name: plan.Name,
                Description: plan.Description,
                Price: plan.Price,
                DurationDays: plan.DurationDays,
                Features: plan.Features,
                IsActive: plan.IsActive,
                CreatedAt: plan.CreatedAt
            );
        }

        private static UserSubscriptionResponse MapSubscriptionToResponse(UserSubscription subscription) {
            return new UserSubscriptionResponse(
                Id: subscription.Id,
                UserId: subscription.UserId,
                Plan: MapPlanToResponse(subscription.SubscriptionPlan),
                StartDate: subscription.StartDate,
                EndDate: subscription.EndDate,
                Status: subscription.Status,
                AutoRenew: subscription.AutoRenew,
                CreatedAt: subscription.CreatedAt
            );
        }
    }
}

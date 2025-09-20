using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class UserSubscriptionRepository : GenericRepository<UserSubscription>, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public async Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(int userId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Plan)
                .FirstOrDefaultAsync(us => us.UserId == userId 
                    && us.IsActive == true 
                    && us.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));
        }

        public async Task<IEnumerable<UserSubscription>> GetSubscriptionHistoryByUserIdAsync(int userId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Plan)
                .Where(us => us.UserId == userId)
                .OrderByDescending(us => us.StartDate)
                .ToListAsync();
        }

        public async Task<bool> DeactivateUserSubscriptionsAsync(int userId)
        {
            var activeSubscriptions = await _context.UserSubscriptions
                .Where(us => us.UserId == userId && us.IsActive == true)
                .ToListAsync();

            if (!activeSubscriptions.Any())
                return true;

            foreach (var subscription in activeSubscriptions)
            {
                subscription.IsActive = false;
            }

            _context.UserSubscriptions.UpdateRange(activeSubscriptions);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsUserSubscriptionActiveAsync(int userId)
        {
            return await _context.UserSubscriptions
                .AnyAsync(us => us.UserId == userId 
                    && us.IsActive == true 
                    && us.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));
        }
    }
}

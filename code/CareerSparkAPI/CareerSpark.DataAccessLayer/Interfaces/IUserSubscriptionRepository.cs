using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription>
    {
        Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(int userId);
        Task<IEnumerable<UserSubscription>> GetSubscriptionHistoryByUserIdAsync(int userId);
        Task<bool> DeactivateUserSubscriptionsAsync(int userId);
        Task<bool> IsUserSubscriptionActiveAsync(int userId);
    }
}

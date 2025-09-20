using CareerSpark.BusinessLayer.DTOs.Response;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<UserSubscriptionResponse?> GetActiveSubscriptionByUserIdAsync(int userId);
        Task<IEnumerable<UserSubscriptionResponse>> GetSubscriptionHistoryByUserIdAsync(int userId);
        Task<bool> DeactivateUserSubscriptionsAsync(int userId);
        Task<bool> IsUserSubscriptionActiveAsync(int userId);
        Task<IEnumerable<UserSubscriptionResponse>> GetAllUserSubscriptionsAsync();
        Task<UserSubscriptionResponse?> GetUserSubscriptionByIdAsync(int id);
    }
}
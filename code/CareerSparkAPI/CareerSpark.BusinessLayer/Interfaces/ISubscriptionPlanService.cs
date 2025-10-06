using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<IEnumerable<SubscriptionPlanResponse>> GetAllSubscriptionPlansAsync();
        Task<IEnumerable<SubscriptionPlanResponse>> GetActiveSubscriptionPlansAsync();
        Task<SubscriptionPlanResponse?> GetSubscriptionPlanByIdAsync(int id);
        Task<SubscriptionPlanResponse?> CreateSubscriptionPlanAsync(SubscriptionPlanRequest request);
        Task<SubscriptionPlanResponse?> UpdateSubscriptionPlanAsync(int id, SubscriptionPlanUpdate request);
        Task<bool> DeleteSubscriptionPlanAsync(int id);
    }
}
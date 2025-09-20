using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<IEnumerable<SubscriptionPlan>> GetActiveSubscriptionPlansAsync();
        Task<SubscriptionPlan?> GetSubscriptionPlanByNameAsync(string name);
        Task<SubscriptionPlan?> GetSubscriptionPlanByIdAsync(int id);
    }
}
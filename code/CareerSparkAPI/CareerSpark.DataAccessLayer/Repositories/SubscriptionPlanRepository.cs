using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetActiveSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
                .Where(sp => sp.IsActive == true)
                .OrderBy(sp => sp.Price)
                .ToListAsync();
        }

        public async Task<SubscriptionPlan?> GetSubscriptionPlanByIdAsync(int id)
        {
            return await _context.SubscriptionPlans.FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<SubscriptionPlan?> GetSubscriptionPlanByNameAsync(string name)
        {
            return await _context.SubscriptionPlans
                .FirstOrDefaultAsync(sp => sp.Name.ToLower() == name.ToLower());
        }
    }
}
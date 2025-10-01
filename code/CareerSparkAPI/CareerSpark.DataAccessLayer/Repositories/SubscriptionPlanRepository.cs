using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

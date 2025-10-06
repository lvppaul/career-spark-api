using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class TestSessionRepository : GenericRepository<TestSession>, ITestSessionRepository
    {
        public TestSessionRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

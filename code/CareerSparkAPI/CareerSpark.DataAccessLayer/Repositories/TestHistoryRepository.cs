using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class TestHistoryRepository : GenericRepository<TestHistory>, ITestHistoryRepository
    {
        public TestHistoryRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

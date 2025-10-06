using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class TestAnswerRepository : GenericRepository<TestAnswer>, ITestAnswerRepository
    {
        public TestAnswerRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

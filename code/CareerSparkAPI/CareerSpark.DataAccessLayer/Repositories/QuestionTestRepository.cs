using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class QuestionTestRepository : GenericRepository<QuestionTest>, IQuestionTestRepository
    {
        public QuestionTestRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

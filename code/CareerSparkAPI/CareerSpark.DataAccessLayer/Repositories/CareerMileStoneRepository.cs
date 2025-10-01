using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CareerMileStoneRepository : GenericRepository<CareerMileStone>, ICareerMileStoneRepository
    {
        public CareerMileStoneRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

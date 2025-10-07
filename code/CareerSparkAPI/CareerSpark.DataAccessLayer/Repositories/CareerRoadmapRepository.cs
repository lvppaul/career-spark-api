using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CareerRoadmapRepository : GenericRepository<CareerRoadmap>, ICareerRoadmapRepository
    {
        public CareerRoadmapRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

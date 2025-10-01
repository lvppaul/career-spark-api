using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CareerFieldRepository : GenericRepository<CareerField>, ICareerFieldRepository
    {
        public CareerFieldRepository(CareerSparkDbContext context) : base(context)
        {
        }
    }
}

using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CareerPathRepository : GenericRepository<CareerPath>, ICareerPathRepository
    {
        public CareerPathRepository(CareerSparkDbContext context) : base(context)
        {
        }
             public async Task<List<CareerPath>> GetAllWithCareerFieldAsync()
        {
            return await _context.CareerPaths
                .Include(cp => cp.CareerField)
                .ToListAsync();
        } 
    }
    
}

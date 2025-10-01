using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CareerMappingRepository : GenericRepository<CareerMapping>, ICareerMappingRepository
    {
        public CareerMappingRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public async Task<CareerMapping?> GetByRiasecTypeAsync(string riasecType)
        {
            return await _context.CareerMappings
                .Include(cm => cm.CareerField)
                .FirstOrDefaultAsync(cm => cm.RiasecType == riasecType);
        }

        public async Task<List<CareerMapping>> GetAllWithFieldAsync()
        {
            return await _context.CareerMappings
                .Include(cm => cm.CareerField)
                .ToListAsync();
        }
    }
}

using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface ICareerMappingRepository : IGenericRepository<CareerMapping>
    {
        Task<CareerMapping?> GetByRiasecTypeAsync(string riasecType);
        Task<List<CareerMapping>> GetAllWithFieldAsync();
    }
}

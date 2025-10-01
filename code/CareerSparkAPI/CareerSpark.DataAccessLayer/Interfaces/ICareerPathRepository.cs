using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface ICareerPathRepository : IGenericRepository<CareerPath>
    {
        Task<List<CareerPath>> GetAllWithCareerFieldAsync();
    }
}

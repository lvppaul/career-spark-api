using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<List<Blog>> GetBlogsByTitleAsync(string title);
        Task<List<Blog>> GetBlogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Blog>> GetRecentBlogsAsync(int count = 10);
        Task<List<Blog>> GetPublishedBlogsAsync();
    }
}

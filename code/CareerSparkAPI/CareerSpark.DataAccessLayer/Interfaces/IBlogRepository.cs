using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<List<Blog>> GetBlogsByTitleAsync(string title);
        Task<List<Blog>> GetBlogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Blog>> GetRecentBlogsAsync(int count = 10);
        Task<List<Blog>> GetPublishedBlogsAsync();
        Task<List<Blog>> GetUnpublishedBlogsAsync();
        Task<PaginatedResult<Blog>> GetUnpublishedBlogsAsyncWithPagination(Pagination pagination);
    }
}

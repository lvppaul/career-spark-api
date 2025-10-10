using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Helper;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogResponse>> GetAllAsync();
        Task<PaginatedResult<BlogResponse>> GetAllAsyncWithPagination(Pagination pagination);
        Task<BlogResponse> GetByIdAsync(int id);
        Task<BlogResponse> CreateAsync(BlogRequest blog);
        Task<BlogResponse> UpdateAsync(int id, BlogUpdate blog);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<BlogResponse>> SearchByTitleAsync(string title);
        Task<IEnumerable<BlogResponse>> GetBlogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<BlogResponse>> GetRecentBlogsAsync(int count = 10);
        Task<IEnumerable<BlogResponse>> GetPublishedBlogsAsync();
        Task<IEnumerable<BlogResponse>> GetUnpublishedBlogsAsync();
        Task<PaginatedResult<BlogResponse>> GetUnpublishedBlogsAsyncWithPagination(Pagination pagination);
        Task<bool> UpdateBlogPublishedAsync(int id);
        Task<bool> UpdateBlogUnpublishedAsync(int id);
        Task<bool> UpdateBlogDeletedAsync(int id);
    }
}

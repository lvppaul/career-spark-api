using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(CareerSparkDbContext context) : base(context)
        {
        }

        private IQueryable<Blog> BaseBlogQuery()
        {
            return _context.Blogs
                .Include(b => b.Comments)
                .Where(b => !b.IsDeleted && b.IsPublished);
        }

        public override async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.Comments) // Include Comments
                .FirstOrDefaultAsync(b => b.Id == id) ?? new Blog();
        }

        public override async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .Include(b => b.Comments)
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.CreateAt) // Order by newest first
                .ToListAsync();
        }

        public override async Task<PaginatedResult<Blog>> GetAllAsyncWithPagination(Pagination pagination)
        {
            var query = BaseBlogQuery();
            // Get total count
            var totalCount = await _context.Blogs.CountAsync();

            // Get paginated items with Comments included
            var items = await query
                .OrderByDescending(b => b.CreateAt) // Order by newest first for consistent pagination
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            return new PaginatedResult<Blog>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        // Blog-specific methods
        public async Task<List<Blog>> GetBlogsByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new List<Blog>();

            return await BaseBlogQuery()
                .Where(b => b.Title.Contains(title))
                .OrderByDescending(b => b.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await BaseBlogQuery()
                .Where(b => b.CreateAt >= startDate && b.CreateAt <= endDate)
                .OrderByDescending(b => b.CreateAt)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetRecentBlogsAsync(int count = 10)
        {
            return await BaseBlogQuery()
                .OrderByDescending(b => b.CreateAt)
                .Take(count)
                .ToListAsync();
        }

        public Task<List<Blog>> GetPublishedBlogsAsync()
        {
            return BaseBlogQuery()
                .OrderByDescending(b => b.CreateAt)
                .ToListAsync();
        }
    }
}

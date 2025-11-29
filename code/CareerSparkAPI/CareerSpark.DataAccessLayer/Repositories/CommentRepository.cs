using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(CareerSparkDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(int blogId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Blog)
                .Where(c => c.BlogId == blogId)
                .OrderByDescending(c => c.CreateAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Blog)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreateAt)
                .ToListAsync();
        }

        public async Task<Comment?> GetCommentWithDetailsAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Blog)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }
    }
}

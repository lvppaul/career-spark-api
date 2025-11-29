using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.DataAccessLayer.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(int blogId);
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
        Task<Comment?> GetCommentWithDetailsAsync(int commentId);
    }
}

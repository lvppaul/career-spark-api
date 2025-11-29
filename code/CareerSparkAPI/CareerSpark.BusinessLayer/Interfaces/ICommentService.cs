using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponse> CreateCommentAsync(CommentRequest request);
        Task<CommentResponse?> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<CommentResponse>> GetCommentsByBlogIdAsync(int blogId);
        Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(int userId);
        Task<CommentResponse?> UpdateCommentAsync(int commentId, string content, int userId);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}

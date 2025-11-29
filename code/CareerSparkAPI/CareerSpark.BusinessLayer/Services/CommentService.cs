using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace CareerSpark.BusinessLayer.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IUnitOfWork unitOfWork, ILogger<CommentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CommentResponse> CreateCommentAsync(CommentRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validate user exists
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("User not found");
                }

                // Validate blog exists and is published
                var blog = await _unitOfWork.BlogRepository.GetByIdAsync(request.BlogId);
                if (blog == null || blog.IsDeleted)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Blog not found or has been deleted");
                }

                if (!blog.IsPublished)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Cannot comment on unpublished blog");
                }

                // Create comment entity
                var comment = CommentMapper.ToEntity(request);

                _unitOfWork.CommentRepository.PrepareCreate(comment);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                // Get comment with details for response
                var commentWithDetails = await _unitOfWork.CommentRepository.GetCommentWithDetailsAsync(comment.Id);

                _logger.LogInformation("Comment {CommentId} created by user {UserId} on blog {BlogId}", 
                    comment.Id, request.UserId, request.BlogId);

                return CommentMapper.ToResponse(commentWithDetails!);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating comment for user {UserId} on blog {BlogId}", 
                    request.UserId, request.BlogId);
                throw;
            }
        }

        public async Task<CommentResponse?> GetCommentByIdAsync(int commentId)
        {
            try
            {
                var comment = await _unitOfWork.CommentRepository.GetCommentWithDetailsAsync(commentId);
                return comment != null ? CommentMapper.ToResponse(comment) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comment by ID {CommentId}", commentId);
                return null;
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByBlogIdAsync(int blogId)
        {
            try
            {
                var comments = await _unitOfWork.CommentRepository.GetCommentsByBlogIdAsync(blogId);
                return CommentMapper.ToResponseList(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments for blog {BlogId}", blogId);
                return Enumerable.Empty<CommentResponse>();
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(int userId)
        {
            try
            {
                var comments = await _unitOfWork.CommentRepository.GetCommentsByUserIdAsync(userId);
                return CommentMapper.ToResponseList(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments for user {UserId}", userId);
                return Enumerable.Empty<CommentResponse>();
            }
        }

        public async Task<CommentResponse?> UpdateCommentAsync(int commentId, string content, int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var comment = await _unitOfWork.CommentRepository.GetCommentWithDetailsAsync(commentId);
                if (comment == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return null;
                }

                // Check if user owns the comment
                if (comment.UserId != userId)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new UnauthorizedAccessException("You can only update your own comments");
                }

                comment.Content = content;
                comment.UpdateAt = DateTime.UtcNow;

                _unitOfWork.CommentRepository.PrepareUpdate(comment);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Comment {CommentId} updated by user {UserId}", commentId, userId);

                return CommentMapper.ToResponse(comment);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating comment {CommentId}", commentId);
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var comment = await _unitOfWork.CommentRepository.GetCommentWithDetailsAsync(commentId);
                if (comment == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Check if user owns the comment
                if (comment.UserId != userId)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new UnauthorizedAccessException("You can only delete your own comments");
                }

                _unitOfWork.CommentRepository.PrepareRemove(comment);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Comment {CommentId} deleted by user {UserId}", commentId, userId);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting comment {CommentId}", commentId);
                throw;
            }
        }
    }
}

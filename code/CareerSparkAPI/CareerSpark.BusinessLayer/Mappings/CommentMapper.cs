using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class CommentMapper
    {
        public static CommentResponse ToResponse(Comment comment)
        {
            return new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = comment.User?.Name ?? string.Empty,
                UserAvatarUrl = comment.User?.avatarURL,
                BlogId = comment.BlogId,
                BlogTitle = comment.Blog?.Title ?? string.Empty,
                CreateAt = comment.CreateAt,
                UpdateAt = comment.UpdateAt
            };
        }

        public static Comment ToEntity(CommentRequest request)
        {
            return new Comment
            {
                Content = request.Content,
                UserId = request.UserId,
                BlogId = request.BlogId,
                CreateAt = DateTime.UtcNow
            };
        }

        public static IEnumerable<CommentResponse> ToResponseList(IEnumerable<Comment> comments)
        {
            return comments.Select(ToResponse).ToList();
        }
    }
}

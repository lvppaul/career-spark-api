using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class BlogMapper
    {
        #region Entity to Response
        public static BlogResponse ToResponse(this Blog blog)
        {
            if (blog == null) return null;
            return new BlogResponse
            {
                Id = blog.Id,
                Title = blog.Title ?? string.Empty,
                Content = blog.Content ?? string.Empty,
                AuthorId = blog.AuthorId,
                CreateAt = blog.CreateAt,
                UpdatedAt = blog.UpdatedAt
            };
        }
        #endregion

        #region Request to Entity
        public static Blog ToEntity(this BlogRequest request)
        {
            if (request == null) return null;

            return new Blog
            {
                Title = request.Title?.Trim(),
                Content = request.Content?.Trim(),
                AuthorId = request.AuthorId,
                CreateAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        #endregion

        #region Request to Updated Entity
        public static void ToUpdate(this BlogUpdate request, Blog blog)
        {
            if (request == null || blog == null) return;

            if (!string.IsNullOrWhiteSpace(request.Title))
                blog.Title = request.Title.Trim();

            if (!string.IsNullOrWhiteSpace(request.Content))
                blog.Content = request.Content.Trim();

            blog.UpdatedAt = DateTime.UtcNow;
        }
        #endregion
    }
}
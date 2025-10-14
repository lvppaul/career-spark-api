
using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class NewsMapper
    {
        #region Entity to Response
        public static NewsResponse ToResponse(this News news)
        {
            if (news == null) return null;
            return new NewsResponse
            {
                Id = news.Id,
                Title = news.Title ?? string.Empty,
                Content = news.Content ?? string.Empty,
                CreatedAt = news.CreatedAt,
                avatarPublicId = news.avatarPublicId,
                ImageUrl = news.ImageUrl,
                IsActive = news.IsActive
            };
        }
        #endregion

        #region Request to Entity
        public static News ToEntity(this NewsRequest request)
        {
            if (request == null) return null;

            return new News
            {
                Title = request.Title?.Trim(),
                Content = request.Content?.Trim(),
                ImageUrl = request.ImageUrl?.Trim(),
                avatarPublicId = request.avatarPublicId?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };
        }
        #endregion
    }
}

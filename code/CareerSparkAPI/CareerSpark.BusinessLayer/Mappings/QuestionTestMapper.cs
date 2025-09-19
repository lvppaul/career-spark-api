using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings;

public static class QuestionTestMapper
{
    public static QuestionTestResponse ToResponse(this QuestionTest entity)
    {
        if (entity == null) return null;
        return new QuestionTestResponse
        {
            Id = entity.Id,
            Content = entity.Content,
            Description = entity.Description,
            QuestionType = entity.QuestionType,
            CreateAt = entity.CreateAt,
            UpdateAt = entity.UpdateAt
        };
    }

    public static QuestionTest ToEntity(this QuestionTestRequest request)
    {
        if (request == null) return null;
        return new QuestionTest
        {
            Content = request.Content.Trim(),
            Description = request.Description?.Trim(),
            QuestionType = request.QuestionType?.Trim(),
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
    }

    public static void ToUpdate(this QuestionTestUpdate update, QuestionTest entity)
    {
        if (update == null || entity == null) return;
        if (!string.IsNullOrWhiteSpace(update.Content)) entity.Content = update.Content.Trim();
        if (update.Description != null) entity.Description = update.Description.Trim();
        if (update.QuestionType != null) entity.QuestionType = update.QuestionType.Trim();
        entity.UpdateAt = DateTime.UtcNow;
    }
}
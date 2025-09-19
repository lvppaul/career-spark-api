using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings;

public static class TestAnswerMapper
{
    public static TestAnswerResponse ToResponse(this TestAnswer entity)
    {
        if (entity == null) return null;
        return new TestAnswerResponse
        {
            Id = entity.Id,
            Content = entity.Content,
            IsSelected = entity.IsSelected,
            QuestionId = entity.QuestionId
        };
    }

    public static TestAnswer ToEntity(this TestAnswerRequest request)
    {
        if (request == null) return null;
        return new TestAnswer
        {
            Content = request.Content.Trim(),
            IsSelected = request.IsSelected,
            QuestionId = request.QuestionId
        };
    }

    public static void ToUpdate(this TestAnswerUpdate update, TestAnswer entity)
    {
        if (update == null || entity == null) return;
        if (!string.IsNullOrWhiteSpace(update.Content)) entity.Content = update.Content.Trim();
        if (update.IsSelected.HasValue) entity.IsSelected = update.IsSelected.Value;
    }
}
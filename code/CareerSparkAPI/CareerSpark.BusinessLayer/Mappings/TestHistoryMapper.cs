using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings;

public static class TestHistoryMapper
{
    public static TestHistoryResponse ToResponse(this TestHistory entity)
    {
        if (entity == null) return null;
        return new TestHistoryResponse
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ResultId = entity.ResultId,
            TestAnswerId = entity.TestAnswerId
        };
    }

    public static TestHistory ToEntity(this TestHistoryRequest request)
    {
        if (request == null) return null;
        return new TestHistory
        {
            UserId = request.UserId,
            ResultId = request.ResultId,
            TestAnswerId = request.TestAnswerId
        };
    }

    public static void ToUpdate(this TestHistoryUpdate update, TestHistory entity)
    {
        if (update == null || entity == null) return;
        if (update.ResultId.HasValue) entity.ResultId = update.ResultId.Value;
        if (update.TestAnswerId.HasValue) entity.TestAnswerId = update.TestAnswerId.Value;
    }
}
using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings;

public static class ResultMapper
{
    public static ResultResponse ToResponse(this Result entity)
    {
        if (entity == null) return null;
        return new ResultResponse
        {
            Id = entity.Id,
            Content = entity.Content,
            R = entity.R,
            I = entity.I,
            A = entity.A,
            S = entity.S,
            E = entity.E,
            C = entity.C
        };
    }

    public static Result ToEntity(this ResultRequest request)
    {
        if (request == null) return null;
        return new Result
        {
            Content = request.Content?.Trim(),
            R = request.R,
            I = request.I,
            A = request.A,
            S = request.S,
            E = request.E,
            C = request.C
        };
    }

    public static void ToUpdate(this ResultUpdate update, Result entity)
    {
        if (update == null || entity == null) return;
        if (update.Content != null) entity.Content = update.Content.Trim();
        if (update.R.HasValue) entity.R = update.R.Value;
        if (update.I.HasValue) entity.I = update.I.Value;
        if (update.A.HasValue) entity.A = update.A.Value;
        if (update.S.HasValue) entity.S = update.S.Value;
        if (update.E.HasValue) entity.E = update.E.Value;
        if (update.C.HasValue) entity.C = update.C.Value;
    }
}
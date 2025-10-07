using System;

namespace CareerSpark.BusinessLayer.DTOs.Response;

public class QuestionTestResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? QuestionType { get; set; }

}
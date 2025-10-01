using System;

namespace CareerSpark.BusinessLayer.DTOs.Response;

public class QuestionTestResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? QuestionType { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
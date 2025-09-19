namespace CareerSpark.BusinessLayer.DTOs.Request;

public class QuestionTestRequest
{
    public string Content { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? QuestionType { get; set; }
}
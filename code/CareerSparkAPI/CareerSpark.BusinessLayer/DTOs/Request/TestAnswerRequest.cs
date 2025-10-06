namespace CareerSpark.BusinessLayer.DTOs.Request;

public class TestAnswerRequest
{
    public string Content { get; set; } = string.Empty;
    public bool? IsSelected { get; set; }
    public int QuestionId { get; set; }
}
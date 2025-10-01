namespace CareerSpark.BusinessLayer.DTOs.Response;

public class TestAnswerResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool? IsSelected { get; set; }
    public int QuestionId { get; set; }
}
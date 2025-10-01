namespace CareerSpark.BusinessLayer.DTOs.Response;

public class TestHistoryResponse
{
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public List<TestHistoryAnswerDto> Answers { get; set; }
}
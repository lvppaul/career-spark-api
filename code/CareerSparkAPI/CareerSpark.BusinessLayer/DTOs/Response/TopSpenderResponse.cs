namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class TopSpenderResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}

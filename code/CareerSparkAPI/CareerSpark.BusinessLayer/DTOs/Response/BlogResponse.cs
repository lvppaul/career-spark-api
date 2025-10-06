namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class BlogResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
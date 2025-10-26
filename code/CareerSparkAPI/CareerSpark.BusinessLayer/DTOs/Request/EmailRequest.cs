namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public string[]? AttachmentFilePaths { get; set; }
    }
}

namespace CareerSpark.DataAccessLayer.Helper
{
    public class TopSpender
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}

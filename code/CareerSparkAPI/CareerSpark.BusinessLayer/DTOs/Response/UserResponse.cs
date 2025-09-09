namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
using CareerSpark.DataAccessLayer.Enums;

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
        public UserRole Role { get; set; }
    }
}
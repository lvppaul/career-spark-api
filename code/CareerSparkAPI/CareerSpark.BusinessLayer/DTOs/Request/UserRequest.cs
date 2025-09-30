using CareerSpark.DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [RegularExpression(@"^(0|\+84)(\d{9})$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public UserRole RoleId { get; set; }


    }
}
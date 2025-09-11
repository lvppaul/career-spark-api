using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Update
{
    public class UserUpdate
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int? RoleId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public bool? IsActive { get; set; }
    }
}
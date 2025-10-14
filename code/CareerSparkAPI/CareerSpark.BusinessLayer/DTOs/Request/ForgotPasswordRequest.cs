using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}

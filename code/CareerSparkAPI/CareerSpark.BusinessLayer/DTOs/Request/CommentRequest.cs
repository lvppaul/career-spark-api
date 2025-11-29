using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 1000 characters")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "BlogId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BlogId must be greater than 0")]
        public int BlogId { get; set; }

        // UserId will be set from JWT token in controller
        public int UserId { get; set; }
    }

    public class UpdateCommentRequest
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 1000 characters")]
        public string Content { get; set; } = string.Empty;
    }
}

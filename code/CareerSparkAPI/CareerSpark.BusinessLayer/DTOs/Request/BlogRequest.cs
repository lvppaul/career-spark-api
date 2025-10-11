using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerSpark.BusinessLayer.DTOs.Request
{
    public class BlogRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Tag cannot exceed 100 characters")]
        public string Tag { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;
        [JsonIgnore]
        public int AuthorId { get; set; }
    }
}
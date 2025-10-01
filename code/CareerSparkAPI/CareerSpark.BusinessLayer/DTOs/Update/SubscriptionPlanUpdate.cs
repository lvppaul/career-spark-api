using System.ComponentModel.DataAnnotations;

namespace CareerSpark.BusinessLayer.DTOs.Update
{
    public class SubscriptionPlanUpdate
    {
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        public int? DurationDays { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}

namespace CareerSpark.BusinessLayer.DTOs.Response
{
    public class SubscriptionPlanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int Level { get; set; }
        public int DurationDays { get; set; }
        public string? Description { get; set; }
    }

    public class UserSubscriptionResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public decimal PlanPrice { get; set; }
        public int PlanDurationDays { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsExpired => EndDate < DateOnly.FromDateTime(DateTime.UtcNow);
        public int RemainingDays => IsActive == true && !IsExpired
            ? (EndDate.ToDateTime(TimeOnly.MinValue) - DateTime.UtcNow).Days
            : 0;
    }
}
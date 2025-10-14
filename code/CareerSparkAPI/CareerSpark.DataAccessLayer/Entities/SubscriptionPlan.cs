namespace CareerSpark.DataAccessLayer.Entities;

public partial class SubscriptionPlan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public string? Description { get; set; }
    public string? Benefits { get; set; }

    public int Level { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}

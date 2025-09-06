using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class SubscriptionPlan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}

using System;
using System.Collections.Generic;

namespace CareerSpark.DataAccessLayer.Entities;

public partial class UserSubscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual SubscriptionPlan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

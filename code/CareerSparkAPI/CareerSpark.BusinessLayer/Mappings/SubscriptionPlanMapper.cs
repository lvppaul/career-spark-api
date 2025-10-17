using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class SubscriptionPlanMapper
    {
        public static SubscriptionPlan ToEntity(this SubscriptionPlanRequest subscriptionPlanRequest)
        {
            return new SubscriptionPlan
            {
                Name = subscriptionPlanRequest.Name,
                Price = subscriptionPlanRequest.Price,
                Level = subscriptionPlanRequest.Level,
                DurationDays = subscriptionPlanRequest.DurationDays,
                Description = subscriptionPlanRequest.Description,
                Benefits = subscriptionPlanRequest.Benefits
            };
        }

        public static SubscriptionPlanResponse ToResponse(SubscriptionPlan subscriptionPlan)
        {
            return new SubscriptionPlanResponse
            {
                Id = subscriptionPlan.Id,
                Name = subscriptionPlan.Name,
                Price = subscriptionPlan.Price,
                Level = subscriptionPlan.Level,
                DurationDays = subscriptionPlan.DurationDays,
                Description = subscriptionPlan.Description,
                Benefits = subscriptionPlan.Benefits ?? string.Empty
            };
        }

        public static IEnumerable<SubscriptionPlanResponse> ToResponseList(IEnumerable<SubscriptionPlan> subscriptionPlans)
        {
            return subscriptionPlans.Select(ToResponse);
        }

        public static UserSubscriptionResponse ToUserSubscriptionResponse(UserSubscription userSubscription)
        {
            return new UserSubscriptionResponse
            {
                Id = userSubscription.Id,
                UserId = userSubscription.UserId,
                PlanId = userSubscription.PlanId,
                PlanName = userSubscription.Plan?.Name ?? string.Empty,
                PlanPrice = userSubscription.Plan?.Price ?? 0,
                PlanDurationDays = userSubscription.Plan?.DurationDays ?? 0,
                Level = userSubscription.Plan?.Level ?? 0,
                Benefits = userSubscription.Plan?.Benefits ?? string.Empty,
                StartDate = userSubscription.StartDate,
                EndDate = userSubscription.EndDate,
                IsActive = userSubscription.IsActive
            };
        }

        public static IEnumerable<UserSubscriptionResponse> ToUserSubscriptionResponseList(IEnumerable<UserSubscription> userSubscriptions)
        {
            return userSubscriptions.Select(ToUserSubscriptionResponse);
        }
    }
}
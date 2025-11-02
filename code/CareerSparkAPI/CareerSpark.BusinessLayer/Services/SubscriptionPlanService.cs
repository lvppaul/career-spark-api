using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace CareerSpark.BusinessLayer.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubscriptionPlanService> _logger;

        public SubscriptionPlanService(IUnitOfWork unitOfWork, ILogger<SubscriptionPlanService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<SubscriptionPlanResponse>> GetAllSubscriptionPlansAsync()
        {
            try
            {
                var subscriptionPlans = await _unitOfWork.SubscriptionPlanRepository.GetAllAsync();
                return SubscriptionPlanMapper.ToResponseList(subscriptionPlans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all subscription plans");
                return Enumerable.Empty<SubscriptionPlanResponse>();
            }
        }

        public async Task<IEnumerable<SubscriptionPlanResponse>> GetActiveSubscriptionPlansAsync()
        {
            try
            {
                var subscriptionPlans = await _unitOfWork.SubscriptionPlanRepository.GetActiveSubscriptionPlansAsync();
                return SubscriptionPlanMapper.ToResponseList(subscriptionPlans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active subscription plans");
                return Enumerable.Empty<SubscriptionPlanResponse>();
            }
        }

        public async Task<SubscriptionPlanResponse?> GetSubscriptionPlanByIdAsync(int id)
        {
            try
            {
                var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
                return subscriptionPlan != null ? SubscriptionPlanMapper.ToResponse(subscriptionPlan) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subscription plan by ID {Id}", id);
                return null;
            }
        }

        public async Task<SubscriptionPlanResponse?> CreateSubscriptionPlanAsync(SubscriptionPlanRequest request)
        {
            try
            {
                // Check if plan with same name already exists
                var existingPlan = await _unitOfWork.SubscriptionPlanRepository.GetSubscriptionPlanByNameAsync(request.Name);
                if (existingPlan != null)
                {
                    throw new InvalidOperationException($"Subscription plan with name '{request.Name}' already exists");
                }

                await _unitOfWork.BeginTransactionAsync();

                var subscriptionPlan = SubscriptionPlanMapper.ToEntity(request);

                _unitOfWork.SubscriptionPlanRepository.PrepareCreate(subscriptionPlan);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return SubscriptionPlanMapper.ToResponse(subscriptionPlan);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating subscription plan");
                throw;
            }
        }

        public async Task<SubscriptionPlanResponse?> UpdateSubscriptionPlanAsync(int id, SubscriptionPlanUpdate request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingPlan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
                if (existingPlan == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException($"Subscription plan with ID {id} not found");
                }

                // Check if new name conflicts with existing plans
                if (!string.IsNullOrWhiteSpace(request.Name) &&
                    !request.Name.Equals(existingPlan.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var planWithSameName = await _unitOfWork.SubscriptionPlanRepository.GetSubscriptionPlanByNameAsync(request.Name);
                    if (planWithSameName != null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw new InvalidOperationException($"Subscription plan with name '{request.Name}' already exists");
                    }
                }

                // Update all fields from the update model
                existingPlan.Name = request.Name;
                existingPlan.Price = request.Price;
                existingPlan.DurationDays = request.DurationDays;
                existingPlan.Level = request.Level;
                existingPlan.Description = request.Description; // allow null/empty to clear description
                existingPlan.Benefits = request.Benefits;
                _unitOfWork.SubscriptionPlanRepository.PrepareUpdate(existingPlan);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return SubscriptionPlanMapper.ToResponse(existingPlan);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating subscription plan with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteSubscriptionPlanAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
                if (subscriptionPlan == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Check if there are active subscriptions using this plan
                var activeSubscriptions = await _unitOfWork.UserSubscriptionRepository.GetAllAsync();
                var hasActiveSubscriptions = activeSubscriptions.Any(us => us.PlanId == id && us.IsActive == true);

                if (hasActiveSubscriptions)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("Cannot delete subscription plan that has active subscriptions");
                }

                subscriptionPlan.IsActive = false;
                _unitOfWork.SubscriptionPlanRepository.PrepareUpdate(subscriptionPlan);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting subscription plan with ID {Id}", id);
                throw;
            }
        }
    }
}
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace CareerSpark.BusinessLayer.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserSubscriptionService> _logger;

        public UserSubscriptionService(IUnitOfWork unitOfWork, ILogger<UserSubscriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserSubscriptionResponse?> GetActiveSubscriptionByUserIdAsync(int userId)
        {
            try
            {
                var subscription = await _unitOfWork.UserSubscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId);
                return subscription != null ? SubscriptionPlanMapper.ToUserSubscriptionResponse(subscription) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active subscription for user {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<UserSubscriptionResponse>> GetSubscriptionHistoryByUserIdAsync(int userId)
        {
            try
            {
                var subscriptions = await _unitOfWork.UserSubscriptionRepository.GetSubscriptionHistoryByUserIdAsync(userId);
                return SubscriptionPlanMapper.ToUserSubscriptionResponseList(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subscription history for user {UserId}", userId);
                return Enumerable.Empty<UserSubscriptionResponse>();
            }
        }

        public async Task<bool> DeactivateUserSubscriptionsAsync(int userId)
        {
            try
            {
                return await _unitOfWork.UserSubscriptionRepository.DeactivateUserSubscriptionsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating subscriptions for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> IsUserSubscriptionActiveAsync(int userId)
        {
            try
            {
                return await _unitOfWork.UserSubscriptionRepository.IsUserSubscriptionActiveAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} has active subscription", userId);
                return false;
            }
        }

        public async Task<IEnumerable<UserSubscriptionResponse>> GetAllUserSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _unitOfWork.UserSubscriptionRepository.GetAllAsync();
                return SubscriptionPlanMapper.ToUserSubscriptionResponseList(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all user subscriptions");
                return Enumerable.Empty<UserSubscriptionResponse>();
            }
        }

        public async Task<UserSubscriptionResponse?> GetUserSubscriptionByIdAsync(int id)
        {
            try
            {
                var subscription = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(id);
                return subscription != null ? SubscriptionPlanMapper.ToUserSubscriptionResponse(subscription) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user subscription by ID {Id}", id);
                return null;
            }
        }
    }
}
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;

        public UserSubscriptionController(IUserSubscriptionService userSubscriptionService)
        {
            _userSubscriptionService = userSubscriptionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUserSubscriptions()
        {
            try
            {
                var subscriptions = await _userSubscriptionService.GetAllUserSubscriptionsAsync();

                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved all user subscriptions",
                    data = subscriptions,
                    count = subscriptions.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving user subscriptions",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSubscriptionById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid subscription ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscription = await _userSubscriptionService.GetUserSubscriptionByIdAsync(id);

                if (subscription == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "User subscription not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User subscription retrieved successfully",
                    data = subscription,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving user subscription",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}/active")]
        public async Task<IActionResult> GetActiveSubscriptionByUserId(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscription = await _userSubscriptionService.GetActiveSubscriptionByUserIdAsync(userId);

                if (subscription == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No active subscription found for this user",
                        data = (object?)null,
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Active subscription retrieved successfully",
                    data = subscription,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving active subscription",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}/history")]
        public async Task<IActionResult> GetSubscriptionHistoryByUserId(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscriptions = await _userSubscriptionService.GetSubscriptionHistoryByUserIdAsync(userId);

                if (subscriptions == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No subscription history found for this user",
                        data = (object?)null,
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Subscription history retrieved successfully",
                    data = subscriptions,
                    count = subscriptions.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving subscription history",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("my-active-subscription")]
        public async Task<IActionResult> GetMyActiveSubscription()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var subscription = await _userSubscriptionService.GetActiveSubscriptionByUserIdAsync(userId);

                if (subscription == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No active subscription found",
                        data = (object?)null,
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Active subscription retrieved successfully",
                    data = subscription,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving your active subscription",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("userId/{userId}/subscription-history")]
        public async Task<IActionResult> GetMySubscriptionHistory(int userId)
        {
            try
            {
                var subscriptions = await _userSubscriptionService.GetSubscriptionHistoryByUserIdAsync(userId);

                if (subscriptions == null || !subscriptions.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No subscription history found",
                        data = (object?)null,
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Subscription history retrieved successfully",
                    data = subscriptions,
                    count = subscriptions.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving your subscription history",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}/status")]
        public async Task<IActionResult> CheckUserSubscriptionStatus(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var isActive = await _userSubscriptionService.IsUserSubscriptionActiveAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = "User subscription status retrieved successfully",
                    data = new
                    {
                        userId = userId,
                        hasActiveSubscription = isActive
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when checking subscription status",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("my-status")]
        public async Task<IActionResult> CheckMySubscriptionStatus()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var isActive = await _userSubscriptionService.IsUserSubscriptionActiveAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = "Your subscription status retrieved successfully",
                    data = new
                    {
                        userId = userId,
                        hasActiveSubscription = isActive
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when checking your subscription status",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user/{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUserSubscriptions(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var result = await _userSubscriptionService.DeactivateUserSubscriptionsAsync(userId);

                if (!result)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to deactivate user subscriptions",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User subscriptions deactivated successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when deactivating user subscriptions",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptionPlans()
        {
            try
            {
                var subscriptionPlans = await _subscriptionPlanService.GetAllSubscriptionPlansAsync();
                if (!subscriptionPlans.Any() || subscriptionPlans == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No subscription plans found",
                        timestamp = DateTime.UtcNow
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved all subscription plans",
                    data = subscriptionPlans,
                    count = subscriptionPlans.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving subscription plans",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveSubscriptionPlans()
        {
            try
            {
                var subscriptionPlans = await _subscriptionPlanService.GetActiveSubscriptionPlansAsync();
                if (!subscriptionPlans.Any() || subscriptionPlans == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No active subscription plans found",
                        timestamp = DateTime.UtcNow
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved active subscription plans",
                    data = subscriptionPlans,
                    count = subscriptionPlans.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving active subscription plans",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionPlanById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid subscription plan ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscriptionPlan = await _subscriptionPlanService.GetSubscriptionPlanByIdAsync(id);

                if (subscriptionPlan == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Subscription plan not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Subscription plan retrieved successfully",
                    data = subscriptionPlan,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving subscription plan",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSubscriptionPlan([FromBody] SubscriptionPlanRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid input data",
                        errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)),
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscriptionPlan = await _subscriptionPlanService.CreateSubscriptionPlanAsync(request);

                return CreatedAtAction(
                    nameof(GetSubscriptionPlanById),
                    new { id = subscriptionPlan.Id },
                    new
                    {
                        success = true,
                        message = "Subscription plan created successfully",
                        data = subscriptionPlan,
                        timestamp = DateTime.UtcNow
                    });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when creating subscription plan",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscriptionPlan(int id, [FromBody] SubscriptionPlanUpdate request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid subscription plan ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid input data",
                        errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)),
                        timestamp = DateTime.UtcNow
                    });
                }

                var subscriptionPlan = await _subscriptionPlanService.UpdateSubscriptionPlanAsync(id, request);
                if (subscriptionPlan == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Updated Subscription plan not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Subscription plan updated successfully",
                    data = subscriptionPlan,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when updating subscription plan",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionPlan(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid subscription plan ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var result = await _subscriptionPlanService.DeleteSubscriptionPlanAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Subscription plan not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Subscription plan deleted successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when deleting subscription plan",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
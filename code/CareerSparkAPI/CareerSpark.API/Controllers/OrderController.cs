using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
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

                // Get user ID from JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid user token",
                        timestamp = DateTime.UtcNow
                    });
                }

                request.UserId = currentUserId;
                // Ensure user can only create orders for themselves (unless admin)
                var userRole = User.FindFirst("Role")?.Value;
                if (userRole == "Admin" && request.UserId != currentUserId)
                {
                    return Forbid();
                }

                var result = await _orderService.CreateOrderAsync(request, HttpContext);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = result.Message,
                        data = result.Order,
                        paymentUrl = result.PaymentUrl,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when creating order",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid order ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Order not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Check if user can access this order
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst("Role")?.Value;

                if (userRole != "Admin" && userRole != "Moderator" &&
                    (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId) ||
                     order.UserId != currentUserId))
                {
                    return Forbid();
                }

                return Ok(new
                {
                    success = true,
                    message = "Order retrieved successfully",
                    data = order,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving order",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
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

                // Check if user can access these orders
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst("Role")?.Value;

                if (userRole != "Admin" && userRole != "Moderator" &&
                    (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId) ||
                     userId != currentUserId))
                {
                    return Forbid();
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = $"Successfully retrieved orders for user {userId}",
                    data = orders,
                    count = orders.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving orders",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid user token",
                        timestamp = DateTime.UtcNow
                    });
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved your orders",
                    data = orders,
                    count = orders.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving your orders",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("cancel-expired")]
        public async Task<IActionResult> CancelExpiredOrders()
        {
            try
            {
                var result = await _orderService.CancelExpiredOrdersAsync();

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Expired orders cancelled successfully",
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to cancel expired orders",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when cancelling expired orders",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
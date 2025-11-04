using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CareerSpark.BusinessLayer.DTOs.Response;

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

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> GetOrdersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? year = null, [FromQuery] int? month = null, [FromQuery] int? day = null)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid pagination parameters",
                        timestamp = DateTime.UtcNow
                    });
                }

                var result = await _orderService.GetOrdersPagedAsync(pageNumber, pageSize, year, month, day);

                return Ok(new
                {
                    success = true,
                    message = "Orders retrieved successfully",
                    data = result.Items,
                    pagination = new { result.TotalCount, result.PageNumber, result.PageSize, result.TotalPages, result.HasPrevious, result.HasNext },
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

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            try
            {
                var total = await _orderService.GetTotalRevenueAsync(start, end);
                return Ok(new
                {
                    success = true,
                    message = "Total revenue retrieved successfully",
                    data = new { total },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving total revenue",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("revenue/years")]
        public async Task<IActionResult> GetRevenueByYear()
        {
            try
            {
                var data = await _orderService.GetRevenueByYearAsync();
                return Ok(new
                {
                    success = true,
                    message = "Revenue by year retrieved successfully",
                    data,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving revenue by year",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("revenue/months")]
        public async Task<IActionResult> GetRevenueByMonth([FromQuery] int year)
        {
            try
            {
                if (year <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid year",
                        timestamp = DateTime.UtcNow
                    });
                }

                var data = await _orderService.GetRevenueByMonthAsync(year);
                return Ok(new
                {
                    success = true,
                    message = "Revenue by month retrieved successfully",
                    data,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving revenue by month",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("revenue/days")]
        public async Task<IActionResult> GetRevenueByDay([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                if (year <= 0 || month <= 0 || month > 12)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid year or month",
                        timestamp = DateTime.UtcNow
                    });
                }

                var data = await _orderService.GetRevenueByDayAsync(year, month);
                return Ok(new
                {
                    success = true,
                    message = "Revenue by day retrieved successfully",
                    data,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving revenue by day",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("top-spenders/current-month")]
        public async Task<IActionResult> GetTopSpendersThisMonth([FromQuery] int top = 10)
        {
            try
            {
                if (top <= 0) top = 10;
                var data = await _orderService.GetTopSpendersThisMonthAsync(top);
                return Ok(new
                {
                    success = true,
                    message = "Top spenders for current month retrieved successfully",
                    data,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving top spenders",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("top-spenders/last-7-days")]
        public async Task<IActionResult> GetTopSpendersLast7Days([FromQuery] int top = 10)
        {
            try
            {
                if (top <= 0) top = 10;
                var data = await _orderService.GetTopSpendersLast7DaysAsync(top);
                return Ok(new
                {
                    success = true,
                    message = "Top spenders for last 7 days retrieved successfully",
                    data,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving top spenders",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
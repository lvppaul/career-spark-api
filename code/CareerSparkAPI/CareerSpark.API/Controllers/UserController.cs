using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("paginated")]
        public async Task<IActionResult> GetAllUsersWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagination = new Pagination(pageNumber, pageSize);
                var result = await _userService.GetAllAsyncWithPagination(pagination);

                // pageNumber vượt total pages
                if (pageNumber > result.TotalPages)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Requested page {pageNumber} exceeds total available pages",
                        details = new
                        {
                            requestedPage = pageNumber,
                            totalPages = result.TotalPages,
                            totalCount = result.TotalCount,
                            pageSize = result.PageSize,
                        },
                        timestamp = DateTime.UtcNow
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = $"Successfully retrieved {result.Items.Count()} users from page {result.PageNumber} of {result.TotalPages}",
                    data = result.Items,
                    pagination = new
                    {
                        totalCount = result.TotalCount,
                        pageNumber = result.PageNumber,
                        pageSize = result.PageSize,
                        totalPages = result.TotalPages,
                        hasPrevious = result.HasPrevious,
                        hasNext = result.HasNext,
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving users",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>User object</returns>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]   // success
        [ProducesResponseType(400)]                 // bad request
        [ProducesResponseType(401)]                 // unauthorized
        [ProducesResponseType(500)]                 // internal server error
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID. ID must be greater than 0",
                        timestamp = DateTime.UtcNow
                    });
                }

                var user = await _userService.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"User with ID {id} not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User retrieved successfully",
                    data = user,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving user",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("setActive/{userID}")]
        public async Task<IActionResult> SetStatus(int userID)
        {

            try
            {
                if (userID <= 0)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user id",
                        timestamp = DateTime.UtcNow
                    });
                var isActive = await _userService.SetActive(userID);
                if (!isActive)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Set active failed",
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Set active successfully",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong when set active User",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Deactive/{userID}")]
        public async Task<IActionResult> Deactive(int userID)
        {

            try
            {
                if (userID <= 0)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user id",
                        timestamp = DateTime.UtcNow
                    });
                var isActive = await _userService.Deactive(userID);

                if (!isActive)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Deactive failed",
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Deactive successfully",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong when set deactive User",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdate userUpdate)
        {
            try
            {

                // Check ModelState for data annotations
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid input data",
                        timestamp = DateTime.UtcNow
                    });
                }

                //  Check if route ID is valid
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid user ID in route",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Check if user exists
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null || existingUser.Id == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "User not found",
                        timestamp = DateTime.UtcNow
                    });
                }
                // Perform the update
                var updatedUser = await _userService.UpdateAsync(id, userUpdate);

                if (updatedUser == null)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to update user information",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User updated successfully",
                    data = updatedUser,
                    timestamp = DateTime.UtcNow
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong happends when updating user information",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}

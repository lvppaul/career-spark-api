using CareerSpark.API.Responses;
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
                        Success = false,
                        Message = $"Requested page {pageNumber} exceeds total available pages",
                        details = new
                        {
                            requestedPage = pageNumber,
                            totalPages = result.TotalPages,
                            totalCount = result.TotalCount,
                            pageSize = result.PageSize,
                        },
                        Timestamp = DateTime.UtcNow
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Message = $"Successfully retrieved {result.Items.Count()} users from page {result.PageNumber} of {result.TotalPages}",
                    Payload = result.Items,
                    pagination = new
                    {
                        totalCount = result.TotalCount,
                        pageNumber = result.PageNumber,
                        pageSize = result.PageSize,
                        totalPages = result.TotalPages,
                        hasPrevious = result.HasPrevious,
                        hasNext = result.HasNext,
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Something went wrong when retrieving users: {ex.Message}",
                    Timestamp = DateTime.UtcNow
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
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid user ID. ID must be greater than 0",
                        Timestamp = DateTime.UtcNow
                    });
                }

                var user = await _userService.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"User with ID {id} not found",
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "User retrieved successfully",
                    Payload = user,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Something went wrong when retrieving user: {ex.Message}",
                    Timestamp = DateTime.UtcNow
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
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid user id",
                        Timestamp = DateTime.UtcNow
                    });
                var isActive = await _userService.SetActive(userID);
                if (!isActive)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Set active failed",
                        Timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Set active successfully",
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Something wrong when set active User: {ex.Message}",
                    Timestamp = DateTime.UtcNow
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
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid user id",
                        Timestamp = DateTime.UtcNow
                    });
                var isActive = await _userService.Deactive(userID);

                if (!isActive)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Deactive failed",
                        Timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Deactive successfully",
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Something wrong when set deactive User: +{ex.Message}",
                    Timestamp = DateTime.UtcNow
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
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid input data",
                        Timestamp = DateTime.UtcNow
                    });
                }

                //  Check if route ID is valid
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid user ID in route",
                        Timestamp = DateTime.UtcNow
                    });
                }

                // Check if user exists
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null || existingUser.Id == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found",
                        Timestamp = DateTime.UtcNow
                    });
                }
                // Perform the update
                var updatedUser = await _userService.UpdateAsync(id, userUpdate);

                if (updatedUser == null)
                {
                    return StatusCode(500, new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to update user information",
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Payload = updatedUser,
                    Timestamp = DateTime.UtcNow
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Something wrong happends when updating user information: + {ex.Message}",
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{userId}/avatar")]
        [Consumes("multipart/form-data")] // Bắt buộc để nhận file
        public async Task<IActionResult> UpdateAvatar(int userId, IFormFile file)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid user ID. ID must be greater than 0",
                        Timestamp = DateTime.UtcNow
                    });
                }
                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null || existingUser.Id == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found",
                        Timestamp = DateTime.UtcNow
                    });
                }
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No file uploaded",
                        Timestamp = DateTime.UtcNow
                    });
                }
                // Upload the file to Cloudinary
                var uploadResult = await _userService.UpdateAvatar(userId, file);
                if (!uploadResult)
                {
                    return StatusCode(500, new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to upload avatar",
                        Timestamp = DateTime.UtcNow
                    });
                }

                var updatedUser = await _userService.GetByIdAsync(userId);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Avatar uploaded and user updated successfully",
                    Payload = new
                    {
                        avatarURL = updatedUser.avatarURL
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Something went wrong when uploading avatar",
                    Timestamp = DateTime.UtcNow
                });
            }

        }
    }
}

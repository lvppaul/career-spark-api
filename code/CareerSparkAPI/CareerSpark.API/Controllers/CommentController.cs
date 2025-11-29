using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Create a new comment on a blog (Authenticated users only)
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest request)
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

                var result = await _commentService.CreateCommentAsync(request);

                return CreatedAtAction(
                    nameof(GetCommentById),
                    new { id = result.Id },
                    new
                    {
                        success = true,
                        message = "Comment created successfully",
                        data = result,
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
                    message = "Something went wrong when creating comment",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get a specific comment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid comment ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment retrieved successfully",
                    data = comment,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving comment",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get all comments for a specific blog (Public - anyone can view)
        /// </summary>
        [HttpGet("blog/{blogId}")]
        public async Task<IActionResult> GetCommentsByBlogId(int blogId)
        {
            try
            {
                if (blogId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid blog ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var comments = await _commentService.GetCommentsByBlogIdAsync(blogId);

                return Ok(new
                {
                    success = true,
                    message = $"Successfully retrieved comments for blog {blogId}",
                    data = comments,
                    count = comments.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving comments",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get all comments by current authenticated user
        /// </summary>
        [Authorize]
        [HttpGet("my-comments")]
        public async Task<IActionResult> GetMyComments()
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

                var comments = await _commentService.GetCommentsByUserIdAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved your comments",
                    data = comments,
                    count = comments.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving your comments",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Update a comment (Only by comment owner)
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid comment ID",
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

                var updatedComment = await _commentService.UpdateCommentAsync(id, request.Content, userId);

                if (updatedComment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment updated successfully",
                    data = updatedComment,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new
                {
                    success = false,
                    message = "You can only update your own comments",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when updating comment",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Delete a comment (Only by comment owner)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid comment ID",
                        timestamp = DateTime.UtcNow
                    });
                }

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

                var result = await _commentService.DeleteCommentAsync(id, userId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment deleted successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new
                {
                    success = false,
                    message = "You can only delete your own comments",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when deleting comment",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get all comments by a specific user (Public)
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
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

                var comments = await _commentService.GetCommentsByUserIdAsync(userId);

                return Ok(new
                {
                    success = true,
                    message = $"Successfully retrieved comments for user {userId}",
                    data = comments,
                    count = comments.Count(),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving user comments",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}

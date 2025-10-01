using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.DataAccessLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("paginated")]
        public async Task<IActionResult> GetAllBlogsWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagination = new Pagination(pageNumber, pageSize);
                var result = await _blogService.GetAllAsyncWithPagination(pagination);

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
                    message = $"Successfully retrieved {result.Items.Count()} blogs from page {result.PageNumber} of {result.TotalPages}",
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
                    message = "Something went wrong when retrieving blogs",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequest blogRequest)
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

                // Create the blog
                var createdBlog = await _blogService.CreateAsync(blogRequest);

                if (createdBlog == null)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to create blog",
                        timestamp = DateTime.UtcNow
                    });
                }

                return CreatedAtAction(
                    nameof(GetBlogById),
                    new { id = createdBlog.Id },
                    new
                    {
                        success = true,
                        message = "Blog created successfully",
                        data = createdBlog,
                        timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong happens when creating blog",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogUpdate blogUpdate)
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
                        message = "Invalid blog ID in route",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Check if blog exists
                var existingBlog = await _blogService.GetByIdAsync(id);
                if (existingBlog == null || existingBlog.Id == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Blog not found",
                        timestamp = DateTime.UtcNow
                    });
                }
                // Perform the update
                var updatedBlog = await _blogService.UpdateAsync(id, blogUpdate);

                if (updatedBlog == null)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to update blog information",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Blog updated successfully",
                    data = updatedBlog,
                    timestamp = DateTime.UtcNow
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong happens when updating blog information",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllAsync();
                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved all blogs",
                    data = blogs,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving blogs",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid blog ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                var blog = await _blogService.GetByIdAsync(id);
                if (blog == null || blog.Id == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Blog not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved blog",
                    data = blog,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving blog",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("BlogPublished")]
        public async Task<IActionResult> GetAllPublishedBlogs()
        {
            try
            {
                var blogs = await _blogService.GetPublishedBlogsAsync();
                return Ok(new
                {
                    success = true,
                    message = "Successfully retrieved all published blogs",
                    data = blogs,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong when retrieving published blogs",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid blog ID",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Check if blog exists
                var existingBlog = await _blogService.GetByIdAsync(id);
                if (existingBlog == null || existingBlog.Id == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Blog not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                // Delete the blog
                var deleteResult = await _blogService.DeleteAsync(id);

                if (!deleteResult)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to delete blog",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Blog deleted successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong happens when deleting blog",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpPatch("{id}/publish")]
        public async Task<IActionResult> PublishBlog(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid blog ID",
                        timestamp = DateTime.UtcNow
                    });
                }
                // Check if blog exists
                var existingBlog = await _blogService.GetByIdAsync(id);
                if (existingBlog == null || existingBlog.Id == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Blog not found",
                        timestamp = DateTime.UtcNow
                    });
                }
                var result = await _blogService.UpdateBlogPublishedAsync(id);
                if (!result)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Failed to publish blog",
                        timestamp = DateTime.UtcNow
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Blog published successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something wrong happens when publishing blog",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
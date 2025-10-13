using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("GetAllActiveNews")]
        public async Task<IActionResult> GetAllActiveNews()
        {
            var news = await _newsService.GetAllActiveNewsAsync();

            return Ok(new
            {
                success = true,
                message = "Successfully retrieved all active news",
                data = news,
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound(new
                {
                    success = false,
                    message = "News not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "Successfully retrieved news by ID",
                data = news,
                timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateNews([FromForm] NewsRequest newsRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid request data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage),
                        timestamp = DateTime.UtcNow
                    });
                }

                var createdNews = await _newsService.CreateNewsAsync(newsRequest);

                return CreatedAtAction(
                        nameof(GetNewsById),
                        new { id = createdNews.Id },
                        new
                        {
                            success = true,
                            message = "News created successfully",
                            data = createdNews,
                            timestamp = DateTime.UtcNow
                        });
            }
            catch (ArgumentException ex)
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
                    message = "An unexpected error occurred while creating news",
                    detail = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpPatch("{id}/deactive")]
        public async Task<IActionResult> DeactiveNews(int id)
        {
            var result = await _newsService.Deactive(id);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "News not found",
                    timestamp = DateTime.UtcNow
                });

            return Ok(new
            {
                success = true,
                message = "News deactivated successfully",
                timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] NewsRequest newsRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid request data",
                        errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage),
                        timestamp = DateTime.UtcNow
                    });
                }

                var updatedNews = await _newsService.UpdateNewsAsync(id, newsRequest);

                if (updatedNews == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "News not found",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "News updated successfully",
                    data = updatedNews,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
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
                    message = "An unexpected error occurred while updating news",
                    detail = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var result = await _newsService.DeleteNewsAsync(id);
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "News not found",
                    timestamp = DateTime.UtcNow
                });

            return NoContent();
        }

    }
}

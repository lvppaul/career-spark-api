using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Update;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.DataAccessLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionTestController : ControllerBase
{
    private readonly IQuestionTestService _service;
    public QuestionTestController(IQuestionTestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(new { success = true, data, timestamp = DateTime.UtcNow });
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagination = new Pagination(pageNumber, pageSize);
        var result = await _service.GetAllAsyncWithPagination(pagination);
        if (pageNumber > result.TotalPages)
        {
            return BadRequest(new
            {
                success = false,
                message = $"Requested page {pageNumber} exceeds total available pages",
                details = new { requestedPage = pageNumber, totalPages = result.TotalPages, totalCount = result.TotalCount, pageSize = result.PageSize },
                timestamp = DateTime.UtcNow
            });
        }
        return Ok(new
        {
            success = true,
            data = result.Items,
            pagination = new { result.TotalCount, result.PageNumber, result.PageSize, result.TotalPages, result.HasPrevious, result.HasNext },
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound(new { success = false, message = "Not found" });
        return Ok(new { success = true, data = item, timestamp = DateTime.UtcNow });
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestionTestRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { success = true, data = created, timestamp = DateTime.UtcNow });
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] QuestionTestUpdate update)
    {
        var updated = await _service.UpdateAsync(id, update);
        return Ok(new { success = true, data = updated, timestamp = DateTime.UtcNow });
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.RemoveAsync(id);
        if (!ok) return NotFound(new { success = false, message = "Delete failed or not found" });
        return Ok(new { success = true, message = "Deleted", timestamp = DateTime.UtcNow });
    }
}

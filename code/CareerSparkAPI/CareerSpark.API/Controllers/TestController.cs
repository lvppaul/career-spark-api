using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var items = await _service.GetQuestionsAsync();
            return Ok(items);
        }

        // lấy ra danh sách các bài test của user
        [HttpGet("sessions/{userId}")]
        public async Task<IActionResult> GetUserSessions(int userId)
        {
            var items = await _service.GetUserTestSessionsAsync(userId);
            return Ok(items);
        }

        // lấy ra test session gần nhất của user (đọc userId từ claim)
        [Authorize]
        [HttpGet("sessions/latest")]
        public async Task<IActionResult> GetMyLatestSession()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var latest = await _service.GetLatestUserTestSessionAsync(userId);
            if (latest == null) return NotFound();
            return Ok(latest);
        }

        // lấy ra test session gần nhất của user (route có userId - legacy)
        [HttpGet("sessions/{userId}/latest")]
        public async Task<IActionResult> GetLatestUserSession(int userId)
        {
            var latest = await _service.GetLatestUserTestSessionAsync(userId);
            if (latest == null) return NotFound();
            return Ok(latest);
        }

        // lấy ra result theo test session id
        [HttpGet("result/{sessionId}")]
        public async Task<IActionResult> GetResultBySession(int sessionId)
        {
            var result = await _service.GetResultBySessionAsync(sessionId);
            return Ok(result);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTest([FromBody] StartTestRequest req)
        {
            var session = await _service.StartTestAsync(req);
            return Ok(session);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitTest([FromBody] SubmitTestRequest req)
        {
            var result = await _service.SubmitTestAsync(req);
            return Ok(result);
        }

        [HttpGet("{sessionId}/roadmap/{userId}")]
        public async Task<IActionResult> GetRoadmap(int sessionId, int userId)
        {
            try
            {
                var roadmap = await _service.GetRoadmapAsync(sessionId, userId);
                return Ok(roadmap);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // lấy ra test detail
        [HttpGet("history/{sessionId}/{userId}")]
        public async Task<IActionResult> GetTestHistory(int sessionId, int userId)
        {
            var result = await _service.GetTestHistoryAsync(sessionId, userId);
            return Ok(result);
        }
    }
}

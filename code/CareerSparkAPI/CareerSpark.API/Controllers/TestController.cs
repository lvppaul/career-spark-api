using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("sessions/{userId}")]
        public async Task<IActionResult> GetUserSessions(int userId)
        {
            var items = await _service.GetUserTestSessionsAsync(userId);
            return Ok(items);
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

        [HttpGet("history/{sessionId}/{userId}")]
        public async Task<IActionResult> GetTestHistory(int sessionId, int userId)
        {
            var result = await _service.GetTestHistoryAsync(sessionId, userId);
            return Ok(result);
        }
    }
}

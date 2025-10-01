using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
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

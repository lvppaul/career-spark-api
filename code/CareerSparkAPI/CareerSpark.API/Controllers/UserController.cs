using CareerSpark.BusinessLayer.Interfaces;
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("setActive/{userID}")]
        public async Task<IActionResult> SetActive(int userID)
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
    }
}

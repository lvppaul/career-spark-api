using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(request);

            if (result == null)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.LoginAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { Success = false, message = "Refresh token is required" });
            }

            var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);

            if (result == null)
            {
                return StatusCode(500, new { Success = false, message = "Internal server error" });
            }

            if (!result.Success)
            {
                return BadRequest(new { Success = false, message = result.Message });
            }

            return Ok(result);
        }

        // Logout thường nghĩa là xóa refreshtoken và refreshTokenAt của user trong DB
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            // Nếu request body trống, lấy access token từ Authorization header
            if (string.IsNullOrWhiteSpace(request.AccessToken) && string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    request.AccessToken = authHeader.Substring("Bearer ".Length).Trim();
                }
            }

            var result = await _authenticationService.LogoutAsync(request);

            if (result == null)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

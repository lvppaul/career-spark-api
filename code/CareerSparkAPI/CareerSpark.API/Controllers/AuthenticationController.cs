using CareerSpark.API.Responses;
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
            var verifyRequest = new ResendVerifyRequest() { Email = request.Email };

            var emailSent = await _authenticationService.VerifyEmailAsync(verifyRequest, CancellationToken.None);
            if (!emailSent)
            {
                return StatusCode(500, new { message = "Failed to send verification email" });
            }

            return CreatedAtAction(
                        nameof(Register),   // Action name
                        new { email = request.Email },  // route values
                        result  // response body
                );
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

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleAccessTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.LoginWithGoogle(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> Verify(ResendVerifyRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.VerifyEmailAsync(request, cancellationToken);

            if (result == null)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }

            if (!result)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to send verification email",
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Please check your mailbox to verify",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.ConfirmEmailAsync(request);
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.ForgotPasswordAsync(request, cancellationToken);
            if (!result)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email not found or not verified. Please verify your email first.",
                    Timestamp = DateTime.UtcNow
                });
            }
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Please check your mailbox to reset password",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.ResetPasswordAsync(request);
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

        //[HttpPost("resend-verification")]
        //public async Task<IActionResult> ResendVerification(ResendVerifyRequest request, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = await _authenticationService.ResendVerifyAsync(request, cancellationToken);
        //    if (result == null)
        //    {
        //        return StatusCode(500, new { message = "Internal server error" });
        //    }
        //    if (!result)
        //    {
        //        return BadRequest(new ApiResponse<object>
        //        {
        //            Success = false,
        //            Message = "Failed to resend verify",
        //            Timestamp = DateTime.UtcNow
        //        });
        //    }
        //    return Ok(new ApiResponse<object>
        //    {
        //        Success = true,
        //        Message = "Please check your mailbox to resend",
        //        Timestamp = DateTime.UtcNow
        //    });
        //}
    }
}

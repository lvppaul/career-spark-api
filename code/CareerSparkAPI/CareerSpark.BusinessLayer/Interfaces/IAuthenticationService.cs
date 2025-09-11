using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;
using System.Security.Claims;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateAccessToken(User user, string roleName);
        string GenerateRefreshToken();
        Task<ClaimsPrincipal?> ValidateToken(string token);

        Task<AuthenticationResponse?> LoginAsync(LoginRequest request);
        Task<AuthenticationResponse?> RefreshTokenAsync(string refreshToken);
        Task<AuthenticationResponse?> RegisterAsync(UserRequest request);
        Task<AuthenticationResponse?> LogoutAsync(LogoutRequest request);
    }
}

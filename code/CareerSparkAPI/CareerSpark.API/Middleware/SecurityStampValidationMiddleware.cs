using CareerSpark.DataAccessLayer.UnitOfWork;
using System.Security.Claims;

namespace CareerSpark.API.Middleware
{
    public class SecurityStampValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public SecurityStampValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        // Danh sách endpoint không cần check SecurityStamp
        private static readonly string[] _excludedPaths = new[]
        {
        "/swagger",
        "/health",
        "/api/public",
        "/favicon.ico",
        "/css",
        "/js",
        "/images",
        "/api/Authentication/login",
        "/api/Authentication/register",
        "/api/Authentication/refreshToken",
        "/api/Authentication/forgot-password",
        "/api/Authentication/reset-password",
        "/api/Authentication/verify-email",
        "/api/Authentication/confirm-email",
        "/api/Authentication/login-google"

        };

        public async Task InvokeAsync(HttpContext context)
        {
            //await using var scope = _serviceProvider.CreateAsyncScope();
            //var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var path = context.Request.Path.Value?.ToLower();

            // Bỏ qua những route không cần kiểm tra
            if (_excludedPaths.Any(path => context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            // Chỉ validate cho requests đã authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var securityStampClaim = context.User.FindFirst("SecurityStamp")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

                    if (user == null || user.SecurityStamp != securityStampClaim)
                    {
                        // Invalid security stamp - clear authentication
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\":\"Security stamp mismatch\"}");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
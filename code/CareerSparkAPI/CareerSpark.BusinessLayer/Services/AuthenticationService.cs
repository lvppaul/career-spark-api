using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Extensions;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Libraries;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Enums;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace CareerSpark.BusinessLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly IUserSubscriptionService _userSubscriptionService;


        public AuthenticationService(
            IHttpClientFactory httpClientFactory,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IEmailService emailService,
            IUserSubscriptionService userSubscriptionService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _secretKey = configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JWT Key not configured");
            _issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JWT Issuer not configured");
            _audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JWT Audience not configured");
            _expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "30");
            _emailService = emailService;
            _userSubscriptionService = userSubscriptionService;
        }


        //GenerateAccessToken
        public string GenerateAccessToken(User user, string roleName, string activeLevelPlan)
        {
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secretKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var email = user.Email;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim("Role", roleName),
                    new Claim ("avatarURL", user.avatarURL ?? string.Empty),
                    new Claim("SubscriptionLevel", activeLevelPlan),
                    // SecurityStamp trong AccessToken để validate mỗi request
                    new Claim("SecurityStamp", user.SecurityStamp)

                }),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = signingCredentials
            };

            var handler = new JsonWebTokenHandler();
            //CreateToken trả về chuỗi đã được mã hóa ( dùng trong JsonWebTokenHandler mới hơn )
            //WriteToken sẽ trả về chuỗi token đã được mã hóa (dùng trong JwtSecurityTokenHandler )
            var token = handler.CreateToken(tokenDescriptor);
            return token;
        }

        //GenerateRefreshToken
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<ClaimsPrincipal?> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JsonWebTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

                //các tham số để xác thực token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero // không cho phép trễ giờ
                };
                //trả về TokenValidationResult
                //TokenValidationResult có các thông tin quan trọng như IsValid, ClaimsIdentity, Exception, SecurityToken
                var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

                // Nếu token hợp lệ, trả về ClaimsPrincipal
                //IsValid: xác định xem token có hợp lệ hay không
                //ClaimsIdentity: chứa các thông tin về người dùng được mã hóa trong token
                if (result.IsValid && result.ClaimsIdentity != null)
                {
                    // Kiểm tra SecurityStamp mỗi request
                    var userIdClaim = result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    var tokenSecurityStamp = result.ClaimsIdentity.FindFirst("SecurityStamp")?.Value;
                    if (int.TryParse(userIdClaim, out int userId) && !string.IsNullOrEmpty(tokenSecurityStamp))
                    {
                        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                        if (user != null && !user.IsSecurityStampValid(tokenSecurityStamp))
                        {
                            // SecurityStamp không khớp -> Token đã bị invalidate
                            return null;
                        }
                    }

                    return new ClaimsPrincipal(result.ClaimsIdentity);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // Đăng nhập và trả về access token và refresh token
        public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Yêu cầu nhập email và mật khẩu"
                };
            }


            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Người dùng không tồn tại! Vui lòng kiểm tra email và thử lại."
                };
            }

            var isVerified = await _unitOfWork.UserRepository.IsEmailVerifiedAsync(user.Id);
            if (!isVerified)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email chưa được xác thực. Vui lòng xác thực email để đăng nhập."
                };
            }

            var isValidPassword = await _unitOfWork.UserRepository.VerifyPasswordAsync(user, request.Password);
            if (!isValidPassword)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Mật khẩu không đúng! Vui lòng kiểm tra và thử lại."
                };
            }
            if (user.IsActive != true)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Tài khoản đang bị vô hiệu hóa"
                };
            }

            var roleName = user.Role?.RoleName;
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Không tìm thấy vai trò của người dùng"
                };
            }

            // Lấy thông tin gói đăng ký hiện tại của user
            var activeSubscription = await _userSubscriptionService.GetActiveSubscriptionByUserIdAsync(user.Id);
            var activeLevelPlan = activeSubscription?.Level ?? 0;
            var accessToken = GenerateAccessToken(user, roleName, activeLevelPlan.ToString());
            var refreshToken = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.ExpiredRefreshTokenAt = refreshTokenExpiry;
            var success = await _unitOfWork.UserRepository.UpdateAsync(user);
            if (success == 0)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Cập nhật refresh token thất bại"
                };
            }

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Data = new AuthenticationData
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        // Làm mới access token bằng cách sử dụng refresh token
        public async Task<AuthenticationResponse?> RefreshTokenAsync(string refreshToken)
        {
            //refreshtoken trống
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Yêu cầu cung cấp refresh token"
                };
            }

            var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken);

            // ko tìm thấy user có refreshtoken nhập vào
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Refresh token không hợp lệ"
                };
            }

            // Kiểm tra xem refresh token đã hết hạn chưa
            // nếu hết hạn thì xóa token
            // và trả về thông báo hết hạn

            if (user.ExpiredRefreshTokenAt == null || user.ExpiredRefreshTokenAt < DateTime.UtcNow)
            {
                // Clear expired token
                user.RefreshToken = null;
                user.ExpiredRefreshTokenAt = null;
                await _unitOfWork.UserRepository.UpdateAsync(user);

                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Refresh token đã hết hạn"
                };
            }

            // Kiểm tra xem tài khoản có bị xóa hay không hoạt động không
            if (user.IsActive != true)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Tài khoản đang bị vô hiệu hóa"
                };
            }

            //Kiểm tra role của user
            var roleName = user.Role?.RoleName;
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Không tìm thấy vai trò của người dùng"
                };
            }

            // Lấy thông tin gói đăng ký hiện tại của user
            var activeSubscription = await _userSubscriptionService.GetActiveSubscriptionByUserIdAsync(user.Id);
            var activeLevelPlan = activeSubscription?.Level ?? 0;

            // nếu chưa hết hạn thì tạo access token mới
            var newAccessToken = GenerateAccessToken(user, roleName, activeLevelPlan.ToString());
            var newRefreshToken = GenerateRefreshToken();
            //var accessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = newRefreshToken;
            user.ExpiredRefreshTokenAt = refreshTokenExpiry;
            await _unitOfWork.UserRepository.UpdateAsync(user);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Làm mới token thành công",
                Data = new AuthenticationData
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };

        }

        // Đăng ký tài khoản mới
        public async Task<AuthenticationResponse?> RegisterAsync(UserRequest request)
        {
            // Validate input Null or Empty
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Name))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Yêu cầu nhập email, mật khẩu và họ tên"
                };
            }

            // Validate email format
            if (!IsValidEmail(request.Email))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Định dạng email không hợp lệ"
                };
            }

            // Validate phone format
            if (!string.IsNullOrWhiteSpace(request.Phone) &&
                !System.Text.RegularExpressions.Regex.IsMatch(request.Phone, @"^(0|\+84)(\d{9})$"))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Định dạng số điện thoại không hợp lệ"
                };
            }
            var phoneExists = await _unitOfWork.UserRepository.GetByPhoneAsync(request.Phone);
            if (phoneExists != null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Số điện thoại đã được sử dụng"
                };
            }

            //Validate password complexity
            var AuthPasswordErrors = ValidatePassword(request.Password);
            // có ? để tránh lỗi null reference
            if (AuthPasswordErrors.Errors?.Any() == true)
            {
                return AuthPasswordErrors;
            }

            // Validate password confirmation
            if (request.Password != request.ConfirmPassword)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Mật khẩu và xác nhận mật khẩu không khớp"
                };
            }

            // Check if user already exists
            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email này đã được đăng ký"
                };
            }

            // Check if role exists and Enum is valid
            if (!Enum.IsDefined(typeof(UserRole), request.RoleId))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"Vai trò không hợp lệ: {request.RoleId}"
                };
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync((int)request.RoleId);
            if (role == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Vai trò không hợp lệ"
                };
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Create new user
                // UserMapper để chuyển từ DTO Request sang Entity nhưng chưa mã hóa password
                // Mã hóa password bằng BCrypt ở đây vì ở đây là tầng quản lý nghiệp vụ và có thể kiểm soát transaction 
                var newUser = UserMapper.ToEntity(request);
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Tạo SecurityStamp mới cho user
                newUser.UpdateSecurityStamp();

                // PrepareCreate để tránh gọi SaveChanges nhiều lần
                // PrepareCreate chỉ thêm entity vào context chứ không lưu vào db ngay lập tức như CreateAsync
                _unitOfWork.UserRepository.PrepareCreate(newUser);
                await _unitOfWork.SaveAsync();

                // Get the created user to have the ID
                var createdUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
                if (createdUser == null)
                {
                    // Trường hợp lẽ ra không xảy ra nhưng vẫn kiểm tra để chắc chắn
                    // Nếu không lấy được user vừa tạo thì rollback transaction và trả về lỗi
                    throw new Exception("Failed to retrieve created user");
                }

                // Generate tokens
                /*   var accessToken = GenerateAccessToken(createdUser, role.RoleName);
                   var refreshToken = GenerateRefreshToken();
                   var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                   // Update user with refresh token
                   createdUser.RefreshToken = refreshToken;
                   createdUser.ExpiredRefreshTokenAt = refreshTokenExpiry
                   // tương tự PrepareCreate
                   _unitOfWork.UserRepository.PrepareUpdate(createdUser);
                   await _unitOfWork.SaveAsync();*/

                await _unitOfWork.CommitTransactionAsync();

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Đăng ký thành công. Vui lòng xác thực email để sử dụng đầy đủ dịch vụ",
                    Data = new AuthenticationData
                    {
                        User = UserMapper.ToResponse(createdUser)
                    }
                };

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"Đăng ký thất bại: {ex.Message}"
                };
            }
        }

        // Logout thường nghĩa là xóa refreshtoken và refreshTokenAt của user trong DB
        public async Task<AuthenticationResponse?> LogoutAsync(LogoutRequest request)
        {
            // Validate input - ít nhất một trong hai token phải có
            if (string.IsNullOrWhiteSpace(request.AccessToken) && string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Cần cung cấp access token hoặc refresh token"
                };
            }

            User? user = null;

            try
            {
                // Nếu có refresh token, tìm user bằng refresh token trước
                if (!string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(request.RefreshToken);
                }

                // Nếu không tìm thấy user bằng refresh token và có access token
                if (user == null && !string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    // Validate access token và lấy user ID từ claims
                    var principal = await ValidateToken(request.AccessToken);
                    if (principal != null)
                    {
                        // lấy ra ID user từ claim "sub"
                        var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                        if (int.TryParse(userIdClaim, out int userId))
                        {
                            user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                        }
                    }
                }

                // Nếu không tìm thấy user
                if (user == null)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Token không hợp lệ hoặc không tìm thấy người dùng"
                    };
                }

                // Kiểm tra xem user có active không
                if (user.IsActive != true)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Tài khoản đang bị vô hiệu hóa"
                    };
                }

                // Clear refresh token và expiry date
                user.RefreshToken = null;
                user.ExpiredRefreshTokenAt = null;

                // Update user trong database
                var updateResult = await _unitOfWork.UserRepository.UpdateAsync(user);
                if (updateResult == 0)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Đăng xuất thất bại"
                    };
                }

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Đăng xuất thành công"
                };
            }
            catch (Exception ex)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"Đăng xuất thất bại: {ex.Message}"
                };
            }
        }

        /*
         Console.WriteLine(IsValidEmail("test@example.com"));   // true
        Console.WriteLine(IsValidEmail("user@domain"));         // vẫn true
        Console.WriteLine(IsValidEmail("user@@domain.com"));    // false (2 ký tự @)
        Console.WriteLine(IsValidEmail("user@domain.com "));    // false (có space cuối)
        Console.WriteLine(IsValidEmail(""));                    // false
         */
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private AuthenticationResponse ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (password.Length < 8)
            {
                errors.Add("Mật khẩu phải có ít nhất 8 ký tự.");
            }

            if (!password.Any(char.IsUpper))
            {
                errors.Add("Mật khẩu phải chứa ít nhất một ký tự hoa.");
            }

            if (!password.Any(char.IsLower))
            {
                errors.Add("Mật khẩu phải chứa ít nhất một ký tự thường.");
            }

            if (!password.Any(char.IsDigit))
            {
                errors.Add("Mật khẩu phải chứa ít nhất một chữ số.");
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors.Add("Mật khẩu phải chứa ít nhất một ký tự đặc biệt.");
            }

            return new AuthenticationResponse
            {
                // error  count = 0 thì success = true
                Success = errors.Count == 0,
                // nếu có lỗi thì trả về danh sách lỗi, không thì trả về null
                Errors = errors.Count > 0 ? errors : null,
                // nếu có lỗi thì trả về thông báo lỗi chung, không thì trả về null
                Message = errors.Count > 0 ? "Xác thực mật khẩu không thành công" : null
            };
        }

        public async Task<AuthenticationResponse> LoginWithGoogle(GoogleAccessTokenRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);

                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                if (!response.IsSuccessStatusCode)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Google access token không hợp lệ"
                    };
                }

                var content = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(content);

                if (userInfo == null || string.IsNullOrEmpty(userInfo.Email))
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Không thể lấy thông tin người dùng từ Google"
                    };
                }

                var user = await _unitOfWork.UserRepository.GetByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    //  Tạo mới user với avatar từ Google
                    user = new User
                    {
                        Name = userInfo.Name,
                        Email = userInfo.Email,
                        IsActive = true,
                        RoleId = (int)UserRole.User,
                        CreatedAt = DateTime.UtcNow,
                        avatarURL = userInfo.Picture,
                        IsVerified = true
                    };

                    _unitOfWork.UserRepository.PrepareCreate(user);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    //  Nếu user chưa có avatar thì cập nhật từ Google
                    if (string.IsNullOrEmpty(user.avatarURL) && !string.IsNullOrEmpty(userInfo.Picture))
                    {
                        user.avatarURL = userInfo.Picture;
                        _unitOfWork.UserRepository.PrepareUpdate(user);
                        await _unitOfWork.SaveAsync();
                    }

                    if (!user.IsActive)
                    {
                        return new AuthenticationResponse
                        {
                            Success = false,
                            Message = "Tài khoản đang bị vô hiệu hóa"
                        };
                    }
                }

                // Lấy thông tin gói đăng ký hiện tại của user
                var activeSubscription = await _userSubscriptionService.GetActiveSubscriptionByUserIdAsync(user.Id);
                var activeLevelPlan = activeSubscription?.Level ?? 0;
                string roleName = user.Role?.RoleName ?? "User";

                // Generate tokens
                var accessToken = GenerateAccessToken(user, roleName, activeLevelPlan.ToString());
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                // Persist refresh token like normal login
                user.RefreshToken = refreshToken;
                user.ExpiredRefreshTokenAt = refreshTokenExpiry;
                _unitOfWork.UserRepository.PrepareUpdate(user);
                await _unitOfWork.SaveAsync();

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Đăng nhập Google thành công",
                    Data = new AuthenticationData
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"Đăng nhập Google thất bại: {ex.Message}"
                };
            }
        }

        public async Task<bool> VerifyEmailAsync(ResendVerifyRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null || user.IsVerified == true)
            {
                return false;
            }

            // Tạo JWT token thay vì DataProtection
            var verificationToken = GenerateEmailVerificationToken(user.Id, user.SecurityStamp);

            var encodedToken = HttpUtility.UrlEncode(verificationToken);
            var confirmUrl = $"{_configuration["Frontend:BaseUrl"]}/confirm-email?email={user.Email!}&token={encodedToken}";


            await _emailService.SendEmailAsync(new EmailRequest
            {
                To = user.Email!,
                Subject = "Confirm your email for register",
                Body = EmailTemplateReader.ConfirmationTemplate(user.Name!, confirmUrl)
            }, cancellationToken);

            return true;
        }

        public async Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            if (user.IsVerified == true)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email đã được xác thực"
                };
            }


            try
            {
                var decodedToken = HttpUtility.UrlDecode(request.Token);
                var (userId, securityStamp) = await ValidateEmailVerificationToken(decodedToken);

                if (userId != user.Id)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Token không hợp lệ"
                    };
                }

                // Kiểm tra SecurityStamp
                if (!user.IsSecurityStampValid(securityStamp))
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Token đã bị vô hiệu hóa. Vui lòng yêu cầu email xác thực mới."
                    };
                }

                await _unitOfWork.BeginTransactionAsync();
                // Xác thực email thành công
                user.IsVerified = true;
                user.InvalidateAllTokens();
                _unitOfWork.UserRepository.PrepareUpdate(user);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Xác thực email thành công"
                };
            }
            catch
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Token không hợp lệ hoặc đã hết hạn"
                };
            }
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            // BUSINESS RULE: Chỉ cho phép reset password khi đã verified
            if (user.IsVerified != true)
            {
                return false; // Không gửi email nếu chưa verify
            }


            // Tạo JWT token cho reset password
            var resetToken = GeneratePasswordResetToken(user.Id, user.SecurityStamp);

            var encodedToken = HttpUtility.UrlEncode(resetToken);
            var resetUrl = $"{_configuration["Frontend:BaseUrl"]}/reset-password?email={HttpUtility.UrlEncode(user.Email!)}&token={encodedToken}";

            await _emailService.SendEmailAsync(new EmailRequest
            {
                To = user.Email!,
                Subject = "Reset Password",
                Body = EmailResetPasswordTemplate.ResetConfirmationTemplate(user.Name!, resetUrl)
            }, cancellationToken);

            return true;
        }

        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            // Business Rule: Chỉ cho phép reset khi đã verified
            if (user.IsVerified != true)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Vui lòng xác thực email trước khi đặt lại mật khẩu"
                };
            }


            try
            {
                var decodedToken = request.Token;
                var (userId, securityStamp) = await ValidatePasswordResetToken(decodedToken);

                if (userId != user.Id)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Token không hợp lệ"
                    };
                }

                // Kiểm tra SecurityStamp
                if (!user.IsSecurityStampValid(securityStamp))
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Token đã bị vô hiệu hóa. Vui lòng yêu cầu email đặt lại mật khẩu mới."
                    };
                }

                // Validate password complexity
                var AuthPasswordErrors = ValidatePassword(request.NewPassword);
                if (AuthPasswordErrors.Errors?.Any() == true)
                {
                    return AuthPasswordErrors;
                }

                // Validate password confirmation
                if (request.NewPassword != request.ConfirmNewPassword)
                {
                    return new AuthenticationResponse
                    {
                        Success = false,
                        Message = "Mật khẩu và xác nhận mật khẩu không khớp"
                    };
                }

                //Validate old and new password
                //if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.Password))
                //{
                //    return new AuthenticationResponse
                //    {
                //        Success = false,
                //        Message = "Mật khẩu mới không được trùng với mật khẩu cũ"
                //    };
                //}

                await _unitOfWork.BeginTransactionAsync();

                // Cập nhật mật khẩu mới đã được mã hóa
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                // Xóa refresh token cũ để buộc đăng nhập lại
                user.InvalidateAllTokens();

                _unitOfWork.UserRepository.PrepareUpdate(user);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Đặt lại mật khẩu thành công"
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"Token không hợp lệ hoặc đã hết hạn: {ex.Message}"
                };
            }
        }

        // Thêm các helper methods
        private string GenerateEmailVerificationToken(int userId, string securityStamp)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", userId.ToString()),
                    new Claim("type", "email_verification"),
                    new Claim("SecurityStamp", securityStamp),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24), // 24 giờ hết hạn
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = signingCredentials
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(tokenDescriptor);
        }

        private string GeneratePasswordResetToken(int userId, string securityStamp)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", userId.ToString()),
                    new Claim("type", "password_reset"),
                    new Claim("SecurityStamp", securityStamp),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30), // 30 phút hết hạn
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = signingCredentials
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(tokenDescriptor);
        }

        private async Task<(int userId, string securityStamp)> ValidateEmailVerificationToken(string token)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            if (!result.IsValid || result.ClaimsIdentity == null)
            {
                throw new SecurityTokenValidationException("Invalid token");
            }

            var typeClaim = result.ClaimsIdentity.FindFirst("type")?.Value;
            if (typeClaim != "email_verification")
            {
                throw new SecurityTokenValidationException("Invalid token type");
            }

            var userIdClaim = result.ClaimsIdentity.FindFirst("sub")?.Value;
            var securityStampClaim = result.ClaimsIdentity.FindFirst("SecurityStamp")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || string.IsNullOrEmpty(securityStampClaim))
            {
                throw new SecurityTokenValidationException("Invalid token claims");
            }


            return (userId, securityStampClaim);
        }

        private async Task<(int userId, string securityStamp)> ValidatePasswordResetToken(string token)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            if (!result.IsValid || result.ClaimsIdentity == null)
            {
                throw new SecurityTokenValidationException("Invalid token");
            }

            var typeClaim = result.ClaimsIdentity.FindFirst("type")?.Value;
            if (typeClaim != "password_reset")
            {
                throw new SecurityTokenValidationException("Invalid token type");
            }

            var userIdClaim = result.ClaimsIdentity.FindFirst("sub")?.Value;
            var securityStampClaim = result.ClaimsIdentity.FindFirst("SecurityStamp")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || string.IsNullOrEmpty(securityStampClaim))
            {
                throw new SecurityTokenValidationException("Invalid token claims");
            }

            return (userId, securityStampClaim);
        }
    }
}

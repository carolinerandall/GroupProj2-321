using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using System.Security.Cryptography;
using System.Text;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(DatabaseService databaseService, ILogger<AuthController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a school user
        /// </summary>
        [HttpPost("school")]
        public async Task<IActionResult> LoginSchool([FromBody] LoginRequest request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                // Hash the password to match stored hash
                var passwordHash = HashPassword(request.Password);

                // Authenticate user
                var school = await _databaseService.AuthenticateSchoolAsync(request.Email, passwordHash);
                if (school == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                _logger.LogInformation($"School login successful for school ID: {school.SchoolId}");
                return Ok(new LoginResponse
                {
                    UserId = school.SchoolId,
                    Email = school.Email,
                    FullName = school.SchoolName,
                    UserType = "school"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during school login");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        /// <summary>
        /// Authenticates a farmer user
        /// </summary>
        [HttpPost("farmer")]
        public async Task<IActionResult> LoginFarmer([FromBody] LoginRequest request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                // Hash the password to match stored hash
                var passwordHash = HashPassword(request.Password);

                // Authenticate user
                var farmer = await _databaseService.AuthenticateFarmerAsync(request.Email, passwordHash);
                if (farmer == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                _logger.LogInformation($"Farmer login successful for farmer ID: {farmer.FarmerId}");
                return Ok(new LoginResponse
                {
                    UserId = farmer.FarmerId,
                    Email = farmer.Email,
                    FullName = farmer.FarmName,
                    UserType = "farmer"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during farmer login");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        /// <summary>
        /// Simple password hashing for MVP (in production, use BCrypt or similar)
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    /// <summary>
    /// Request model for login
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for successful login
    /// </summary>
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }
}


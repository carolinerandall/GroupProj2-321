using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using System.Security.Cryptography;
using System.Text;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(DatabaseService databaseService, ILogger<UsersController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new school account
        /// </summary>
        [HttpPost("school")]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolSignUpRequest request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email) || 
                    string.IsNullOrWhiteSpace(request.Password) || 
                    string.IsNullOrWhiteSpace(request.SchoolName) ||
                    string.IsNullOrWhiteSpace(request.ContactName))
                {
                    return BadRequest("Email, password, school name, and contact name are required.");
                }

                // Check if email already exists
                if (await _databaseService.SchoolEmailExistsAsync(request.Email))
                {
                    return BadRequest("An account with this email already exists.");
                }

                // Hash the password (simple hash for MVP - in production use BCrypt or similar)
                var passwordHash = HashPassword(request.Password);

                // Create school account
                var schoolId = await _databaseService.CreateSchoolAsync(
                    request.SchoolName,
                    request.ContactName,
                    request.Email, 
                    passwordHash,
                    request.Phone,
                    request.Address,
                    request.City,
                    request.State,
                    request.ZipCode);

                _logger.LogInformation($"School account created successfully for school ID: {schoolId}");
                return Ok(new { message = "School account created successfully", schoolId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating school account");
                return StatusCode(500, "An error occurred while creating the school account.");
            }
        }

        /// <summary>
        /// Creates a new farmer account
        /// </summary>
        [HttpPost("farmer")]
        public async Task<IActionResult> CreateFarmer([FromBody] FarmerSignUpRequest request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email) || 
                    string.IsNullOrWhiteSpace(request.Password) || 
                    string.IsNullOrWhiteSpace(request.FarmName) ||
                    string.IsNullOrWhiteSpace(request.FirstName) ||
                    string.IsNullOrWhiteSpace(request.LastName))
                {
                    return BadRequest("Email, password, farm name, first name, and last name are required.");
                }

                // Check if email already exists
                if (await _databaseService.FarmerEmailExistsAsync(request.Email))
                {
                    return BadRequest("An account with this email already exists.");
                }

                // Hash the password
                var passwordHash = HashPassword(request.Password);

                // Create farmer account
                var farmerId = await _databaseService.CreateFarmerAsync(
                    request.FarmName,
                    request.FirstName,
                    request.LastName,
                    request.Email, 
                    passwordHash,
                    request.Phone,
                    request.Address,
                    request.City,
                    request.State,
                    request.ZipCode);

                _logger.LogInformation($"Farmer account created successfully for farmer ID: {farmerId}");
                return Ok(new { message = "Farmer account created successfully", farmerId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating farmer account");
                return StatusCode(500, "An error occurred while creating the farmer account.");
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
    /// Request model for school sign up
    /// </summary>
    public class SchoolSignUpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }

    /// <summary>
    /// Request model for farmer sign up
    /// </summary>
    public class FarmerSignUpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FarmName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}


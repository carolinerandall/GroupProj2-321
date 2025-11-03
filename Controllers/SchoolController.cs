using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using GroupProj2_321.Models;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(DatabaseService databaseService, ILogger<SchoolController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Gets school profile by school ID
        /// </summary>
        [HttpGet("profile/{schoolId}")]
        public async Task<IActionResult> GetSchoolProfile(int schoolId)
        {
            try
            {
                var school = await _databaseService.GetSchoolProfileAsync(schoolId);
                if (school == null)
                {
                    return NotFound("School profile not found.");
                }

                return Ok(school);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting school profile for school ID: {SchoolId}", schoolId);
                return StatusCode(500, "An error occurred while retrieving the school profile.");
            }
        }

        /// <summary>
        /// Updates school profile
        /// </summary>
        [HttpPut("profile/{schoolId}")]
        public async Task<IActionResult> UpdateSchoolProfile(int schoolId, [FromBody] UpdateSchoolProfileRequest request)
        {
            try
            {
                // Validate required fields
                if (request == null)
                {
                    return BadRequest("Profile data is required.");
                }

                await _databaseService.UpdateSchoolProfileAsync(
                    schoolId,
                    request.SchoolName,
                    request.ContactName,
                    request.Phone,
                    request.Address,
                    request.City,
                    request.State,
                    request.ZipCode
                );

                _logger.LogInformation($"School profile updated for school ID: {schoolId}");
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating school profile for school ID: {SchoolId}", schoolId);
                return StatusCode(500, "An error occurred while updating the profile.");
            }
        }

        /// <summary>
        /// Gets available produce items for browsing
        /// </summary>
        [HttpGet("produce/available")]
        public async Task<IActionResult> GetAvailableProduce([FromQuery] string? produceName = null, [FromQuery] int? farmerId = null)
        {
            try
            {
                var produce = await _databaseService.GetAvailableProduceAsync(produceName, farmerId);
                return Ok(produce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available produce. Error: {Message}", ex.Message);
                return StatusCode(500, $"An error occurred while retrieving available produce: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets school's orders
        /// </summary>
        [HttpGet("{schoolId}/orders")]
        public async Task<IActionResult> GetSchoolOrders(int schoolId)
        {
            try
            {
                var orders = await _databaseService.GetSchoolOrdersAsync(schoolId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for school ID: {SchoolId}. Error: {Message}", schoolId, ex.Message);
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Request model for updating school profile
    /// </summary>
    public class UpdateSchoolProfileRequest
    {
        public string? SchoolName { get; set; }
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}


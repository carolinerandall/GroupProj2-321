using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using GroupProj2_321.Models;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<FarmerController> _logger;

        public FarmerController(DatabaseService databaseService, ILogger<FarmerController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Gets farmer profile by farmer ID
        /// </summary>
        [HttpGet("profile/{farmerId}")]
        public async Task<IActionResult> GetFarmerProfile(int farmerId)
        {
            try
            {
                var farmer = await _databaseService.GetFarmerProfileAsync(farmerId);
                if (farmer == null)
                {
                    return NotFound("Farmer profile not found.");
                }

                return Ok(farmer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting farmer profile for farmer ID: {FarmerId}", farmerId);
                return StatusCode(500, "An error occurred while retrieving the farmer profile.");
            }
        }

        /// <summary>
        /// Updates farmer profile
        /// </summary>
        [HttpPut("profile/{farmerId}")]
        public async Task<IActionResult> UpdateFarmerProfile(int farmerId, [FromBody] UpdateFarmerProfileRequest request)
        {
            try
            {
                // Validate required fields
                if (request == null)
                {
                    return BadRequest("Profile data is required.");
                }

                await _databaseService.UpdateFarmerProfileAsync(
                    farmerId,
                    request.FarmName,
                    request.FirstName,
                    request.LastName,
                    request.Phone,
                    request.Address,
                    request.City,
                    request.State,
                    request.ZipCode
                );

                _logger.LogInformation($"Farmer profile updated for farmer ID: {farmerId}");
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating farmer profile for farmer ID: {FarmerId}", farmerId);
                return StatusCode(500, "An error occurred while updating the profile.");
            }
        }

        /// <summary>
        /// Gets all produce items for a farmer
        /// </summary>
        [HttpGet("{farmerId}/produce")]
        public async Task<IActionResult> GetFarmerProduce(int farmerId)
        {
            try
            {
                var produce = await _databaseService.GetFarmerProduceAsync(farmerId);
                return Ok(produce);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting produce for farmer ID: {FarmerId}", farmerId);
                return StatusCode(500, "An error occurred while retrieving produce.");
            }
        }

        /// <summary>
        /// Adds a new produce item
        /// </summary>
        [HttpPost("{farmerId}/produce")]
        public async Task<IActionResult> AddProduce(int farmerId, [FromBody] AddProduceRequest request)
        {
            try
            {
                // Validate required fields
                if (request == null || string.IsNullOrWhiteSpace(request.ProduceName) || request.PricePerUnit <= 0)
                {
                    return BadRequest("Produce name and valid price per unit are required.");
                }

                var produceId = await _databaseService.AddProduceAsync(
                    farmerId,
                    request.ProduceName,
                    request.Description,
                    request.PricePerUnit,
                    request.AvailableQuantity,
                    request.Unit,
                    request.FarmerCanDeliver,
                    request.AvailabilityStart,
                    request.AvailabilityEnd
                );

                _logger.LogInformation($"Produce added for farmer ID: {farmerId}, produce ID: {produceId}");
                return Ok(new { message = "Produce added successfully.", produceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding produce for farmer ID: {FarmerId}", farmerId);
                return StatusCode(500, "An error occurred while adding produce.");
            }
        }

        /// <summary>
        /// Updates produce details
        /// </summary>
        [HttpPut("produce/{produceId}")]
        public async Task<IActionResult> UpdateProduce(int produceId, [FromBody] UpdateProduceRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Produce data is required.");
                }

                await _databaseService.UpdateProduceAsync(
                    produceId,
                    request.ProduceName,
                    request.Description,
                    request.PricePerUnit,
                    request.AvailableQuantity,
                    request.Unit,
                    request.FarmerCanDeliver,
                    request.AvailabilityStart,
                    request.AvailabilityEnd
                );

                _logger.LogInformation($"Produce updated for produce ID: {produceId}");
                return Ok(new { message = "Produce updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating produce for produce ID: {ProduceId}", produceId);
                return StatusCode(500, "An error occurred while updating produce.");
            }
        }

        /// <summary>
        /// Gets farmer's orders
        /// </summary>
        [HttpGet("{farmerId}/orders")]
        public async Task<IActionResult> GetFarmerOrders(int farmerId)
        {
            try
            {
                var orders = await _databaseService.GetFarmerOrdersAsync(farmerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for farmer ID: {FarmerId}", farmerId);
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }
    }

    /// <summary>
    /// Request model for updating farmer profile
    /// </summary>
    public class UpdateFarmerProfileRequest
    {
        public string? FarmName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }

    /// <summary>
    /// Request model for adding produce
    /// </summary>
    public class AddProduceRequest
    {
        public string ProduceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PricePerUnit { get; set; }
        public int AvailableQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool FarmerCanDeliver { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailabilityEnd { get; set; }
    }

    /// <summary>
    /// Request model for updating produce
    /// </summary>
    public class UpdateProduceRequest
    {
        public string? ProduceName { get; set; }
        public string? Description { get; set; }
        public decimal? PricePerUnit { get; set; }
        public int? AvailableQuantity { get; set; }
        public string? Unit { get; set; }
        public bool? FarmerCanDeliver { get; set; }
        public DateTime? AvailabilityStart { get; set; }
        public DateTime? AvailabilityEnd { get; set; }
    }
}


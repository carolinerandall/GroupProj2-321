using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using GroupProj2_321.Models;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(DatabaseService databaseService, ILogger<DeliveriesController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Creates or assigns a delivery for an order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequest request)
        {
            try
            {
                if (request == null || request.OrderId <= 0)
                {
                    return BadRequest("Valid order ID is required.");
                }

                var deliveryId = await _databaseService.CreateDeliveryAsync(
                    request.OrderId,
                    request.TruckCompany,
                    request.TruckContact,
                    request.DeliveryFeeTotal,
                    request.EstimatedArrival
                );

                _logger.LogInformation($"Delivery created successfully with ID: {deliveryId} for order {request.OrderId}");
                return Ok(new { message = "Delivery created successfully", deliveryId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery for order ID: {OrderId}", request?.OrderId);
                return StatusCode(500, "An error occurred while creating the delivery.");
            }
        }

        /// <summary>
        /// Gets delivery details by order ID
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetDeliveryByOrderId(int orderId)
        {
            try
            {
                var delivery = await _databaseService.GetDeliveryByOrderIdAsync(orderId);
                if (delivery == null)
                {
                    return NotFound("Delivery not found.");
                }

                return Ok(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving delivery for order ID: {OrderId}", orderId);
                return StatusCode(500, "An error occurred while retrieving the delivery.");
            }
        }

        /// <summary>
        /// Updates delivery status
        /// </summary>
        [HttpPut("{deliveryId}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int deliveryId, [FromBody] UpdateDeliveryStatusRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DeliveryStatus))
                {
                    return BadRequest("Delivery status is required.");
                }

                await _databaseService.UpdateDeliveryStatusAsync(deliveryId, request.DeliveryStatus);
                _logger.LogInformation($"Delivery {deliveryId} status updated to {request.DeliveryStatus}");
                return Ok(new { message = "Delivery status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery status for delivery {DeliveryId}", deliveryId);
                return StatusCode(500, "An error occurred while updating the delivery status.");
            }
        }
    }

    public class CreateDeliveryRequest
    {
        public int OrderId { get; set; }
        public string? TruckCompany { get; set; }
        public string? TruckContact { get; set; }
        public decimal DeliveryFeeTotal { get; set; }
        public DateTime EstimatedArrival { get; set; }
    }

    public class UpdateDeliveryStatusRequest
    {
        public string DeliveryStatus { get; set; } = string.Empty;
    }
}


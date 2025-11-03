using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Models;
using GroupProj2_321.Services;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(DatabaseService databaseService, ILogger<PaymentController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a payment for an order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                _logger.LogInformation("Creating payment for order ID: {OrderId}", request.OrderId);

                // Validate request
                if (request.OrderId <= 0 || request.Amount <= 0)
                {
                    return BadRequest("Invalid payment data");
                }

                // Create payment record
                var payment = new Payment
                {
                    OrderId = request.OrderId,
                    Amount = request.Amount,
                    PaymentMethod = request.PaymentMethod ?? "Credit Card",
                    TransactionId = request.TransactionId ?? $"mock_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                    PaymentDate = DateTime.UtcNow,
                    Status = "Successful" // Mock payment always succeeds
                };

                var paymentId = await _databaseService.CreatePaymentAsync(payment);

                // Update order payment status
                await _databaseService.UpdateOrderPaymentStatusAsync(request.OrderId, "Paid");

                _logger.LogInformation("Payment created successfully with ID: {PaymentId}", paymentId);

                return Ok(new { PaymentId = paymentId, Message = "Payment processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for order ID: {OrderId}", request.OrderId);
                return StatusCode(500, "An error occurred while processing the payment");
            }
        }

        /// <summary>
        /// Gets payment by order ID
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var payment = await _databaseService.GetPaymentByOrderIdAsync(orderId);
                
                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment for order ID: {OrderId}", orderId);
                return StatusCode(500, "An error occurred while retrieving the payment");
            }
        }
    }

    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }
}


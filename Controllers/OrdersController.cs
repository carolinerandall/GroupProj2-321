using Microsoft.AspNetCore.Mvc;
using GroupProj2_321.Services;
using GroupProj2_321.Models;

namespace GroupProj2_321.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(DatabaseService databaseService, ILogger<OrdersController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (request == null || request.SchoolId <= 0 || request.FarmerId <= 0 || request.OrderItems == null || request.OrderItems.Count == 0)
                {
                    return BadRequest("Valid school ID, farmer ID, and order items are required.");
                }

                var orderId = await _databaseService.CreateOrderAsync(
                    request.SchoolId,
                    request.FarmerId,
                    request.OrderDate,
                    request.DeliveryDate,
                    request.OrderItems
                );

                _logger.LogInformation($"Order created successfully with ID: {orderId} for school {request.SchoolId}");
                return Ok(new { message = "Order created successfully", orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order. Error: {Message}", ex.Message);
                return StatusCode(500, new { message = $"An error occurred while creating the order: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gets order details by order ID
        /// </summary>
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var order = await _databaseService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order {OrderId}", orderId);
                return StatusCode(500, "An error occurred while retrieving the order.");
            }
        }

        /// <summary>
        /// Gets school's orders
        /// </summary>
        [HttpGet("school/{schoolId:int}")]
        public async Task<IActionResult> GetSchoolOrders(int schoolId)
        {
            try
            {
                var orders = await _databaseService.GetSchoolOrdersAsync(schoolId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for school ID: {SchoolId}. Error: {Message}", schoolId, ex.Message);
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets farmer's orders
        /// </summary>
        [HttpGet("farmer/{farmerId:int}")]
        public async Task<IActionResult> GetFarmerOrders(int farmerId)
        {
            try
            {
                var orders = await _databaseService.GetFarmerOrdersAsync(farmerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for farmer ID: {FarmerId}. Error: {Message}", farmerId, ex.Message);
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates order status
        /// </summary>
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Status))
                {
                    return BadRequest("Status is required.");
                }

                await _databaseService.UpdateOrderStatusAsync(orderId, request.Status);
                _logger.LogInformation($"Order {orderId} status updated to {request.Status}");
                return Ok(new { message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status for order {OrderId}", orderId);
                return StatusCode(500, "An error occurred while updating the order status.");
            }
        }

        /// <summary>
        /// Updates order payment and delivery fee
        /// </summary>
        [HttpPut("{orderId:int}/payment")]
        public async Task<IActionResult> UpdateOrderPayment(int orderId, [FromBody] UpdateOrderPaymentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Payment data is required.");
                }

                await _databaseService.UpdateOrderPaymentAndDeliveryAsync(
                    orderId,
                    "Paid",
                    request.DeliveryFee ?? 0,
                    request.CreditCardLast4
                );

                // Create payment record
                if (request.Amount > 0)
                {
                    var payment = new Payment
                    {
                        OrderId = orderId,
                        Amount = request.Amount,
                        PaymentMethod = "Credit Card",
                        TransactionId = $"card_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                        PaymentDate = DateTime.UtcNow,
                        Status = "Successful"
                    };

                    await _databaseService.CreatePaymentAsync(payment);
                }

                _logger.LogInformation($"Order {orderId} payment updated successfully");
                return Ok(new { message = "Payment information updated successfully. Waiting on farmer to complete order." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment for order {OrderId}", orderId);
                return StatusCode(500, $"An error occurred while updating payment: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        [HttpPost("{orderId:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = await _databaseService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                if (order.Status == "Cancelled")
                {
                    return BadRequest("Order is already cancelled.");
                }

                if (order.Status == "Delivered")
                {
                    return BadRequest("Cannot cancel a delivered order.");
                }

                await _databaseService.UpdateOrderStatusAsync(orderId, "Cancelled");
                _logger.LogInformation($"Order {orderId} cancelled successfully");
                return Ok(new { message = "Order cancelled successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
                return StatusCode(500, "An error occurred while cancelling the order.");
            }
        }
    }

    public class CreateOrderRequest
    {
        public int SchoolId { get; set; }
        public int FarmerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; } = new List<OrderItemRequest>();
    }

    public class OrderItemRequest
    {
        public int ProduceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateOrderPaymentRequest
    {
        public decimal Amount { get; set; }
        public decimal? DeliveryFee { get; set; }
        public string? CreditCardLast4 { get; set; }
    }
}


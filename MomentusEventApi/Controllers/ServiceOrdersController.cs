using Microsoft.AspNetCore.Mvc;
using MomentusEventApi.Models;
using MomentusEventApi.Services;

namespace MomentusEventApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderService _orderService;
    private readonly ILogger<ServiceOrdersController> _logger;

    public ServiceOrdersController(IServiceOrderService orderService, ILogger<ServiceOrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Get all service orders for a specific event
    /// </summary>
    /// <param name="eventId">The event ID to get orders for</param>
    /// <returns>List of service orders for the event</returns>
    [HttpGet("event/{eventId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ServiceOrder>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetOrdersByEvent(int eventId)
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync(eventId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service orders for event {EventId}", eventId);
            return StatusCode(500, "An error occurred while retrieving service orders");
        }
    }

    /// <summary>
    /// Get a specific service order by order number
    /// </summary>
    /// <param name="orderNumber">The order number</param>
    /// <returns>Service order details</returns>
    [HttpGet("{orderNumber:int}")]
    [ProducesResponseType(typeof(ServiceOrder), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceOrder>> GetOrderByNumber(int orderNumber)
    {
        try
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            
            if (order == null)
            {
                return NotFound($"Service order with number {orderNumber} not found");
            }
            
            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service order {OrderNumber}", orderNumber);
            return StatusCode(500, "An error occurred while retrieving the service order");
        }
    }

    /// <summary>
    /// Search for service orders using Ungerboeck search filter syntax
    /// </summary>
    /// <param name="filter">OData-style search filter (e.g., "Account eq 'TECHCONF'")</param>
    /// <returns>List of matching service orders</returns>
    /// <remarks>
    /// Examples of search filters:
    /// - Account eq 'TECHCONF'
    /// - OrderStatus eq 'A'
    /// - Event eq 1
    /// - StartDate gt 2026-01-01
    /// Leave empty to return all orders
    /// </remarks>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ServiceOrder>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> SearchOrders([FromQuery] string? filter = null)
    {
        try
        {
            var orders = await _orderService.SearchOrdersAsync(filter ?? "");
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching service orders with filter: {Filter}", filter);
            return StatusCode(500, "An error occurred while searching service orders");
        }
    }
}

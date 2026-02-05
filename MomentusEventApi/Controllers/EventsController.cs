using Microsoft.AspNetCore.Mvc;
using MomentusEventApi.Models;
using MomentusEventApi.Services;

namespace MomentusEventApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMomentusEventService _eventService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IMomentusEventService eventService, ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    /// <summary>
    /// Get all events from Momentus
    /// </summary>
    /// <returns>List of all events</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        try
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all events");
            return StatusCode(500, "An error occurred while retrieving events");
        }
    }

    /// <summary>
    /// Get a specific event by ID from Momentus
    /// </summary>
    /// <param name="id">The event ID</param>
    /// <returns>Event details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        try
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            
            if (eventItem == null)
            {
                return NotFound($"Event with ID {id} not found");
            }
            
            return Ok(eventItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event {EventId}", id);
            return StatusCode(500, "An error occurred while retrieving the event");
        }
    }
}

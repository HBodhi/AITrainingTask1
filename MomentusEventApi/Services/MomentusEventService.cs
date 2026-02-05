using MomentusEventApi.Models;

namespace MomentusEventApi.Services;

public class MomentusEventService : IMomentusEventService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MomentusEventService> _logger;
    private readonly string _momentusApiBaseUrl;

    public MomentusEventService(HttpClient httpClient, ILogger<MomentusEventService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _momentusApiBaseUrl = configuration["MomentusApi:BaseUrl"] ?? "https://api.momentus.com";
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all events from Momentus API");
            
            // For now, returning mock data since we don't have the actual Momentus API endpoint
            // In production, this would call the actual Momentus API
            // var response = await _httpClient.GetFromJsonAsync<IEnumerable<Event>>($"{_momentusApiBaseUrl}/events");
            
            return GetMockEvents();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching events from Momentus API");
            throw;
        }
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching event {EventId} from Momentus API", id);
            
            // For now, returning mock data
            // In production: var response = await _httpClient.GetFromJsonAsync<Event>($"{_momentusApiBaseUrl}/events/{id}");
            
            var events = GetMockEvents();
            return await Task.FromResult(events.FirstOrDefault(e => e.EventID == id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching event {EventId} from Momentus API", id);
            throw;
        }
    }

    private IEnumerable<Event> GetMockEvents()
    {
        // Mock data simulating Momentus event data using EventsModel structure
        return new List<Event>
        {
            new Event
            {
                EventID = 1,
                Organization = "10",
                Description = "Tech Conference 2026",
                Account = "TECHCONF",
                StartDate = new DateTime(2026, 6, 15, 9, 0, 0),
                EndDate = new DateTime(2026, 6, 17, 18, 0, 0),
                StartTime = new DateTime(2026, 6, 15, 9, 0, 0),
                EndTime = new DateTime(2026, 6, 17, 18, 0, 0),
                Status = "30", // Firm status
                Type = "EDU",
                Category = "CO",
                Attendance = 500,
                ForecastAttendance = 500,
                ForecastRevenue = 299990,
                Description1 = "Annual technology conference featuring the latest innovations",
                Description2 = "San Francisco, CA",
                Coordinator = "TECHCOORD",
                Contact = "TECHCONT"
            },
            new Event
            {
                EventID = 2,
                Organization = "10",
                Description = "Music Festival 2026",
                Account = "MUSICFEST",
                StartDate = new DateTime(2026, 7, 20, 12, 0, 0),
                EndDate = new DateTime(2026, 7, 22, 23, 0, 0),
                StartTime = new DateTime(2026, 7, 20, 12, 0, 0),
                EndTime = new DateTime(2026, 7, 22, 23, 0, 0),
                Status = "30",
                Type = "ENT",
                Category = "MU",
                Attendance = 10000,
                ForecastAttendance = 10000,
                ForecastRevenue = 1499900,
                Description1 = "Three-day outdoor music festival with top artists",
                Description2 = "Austin, TX",
                Coordinator = "MUSICCOORD",
                Contact = "MUSICCONT"
            },
            new Event
            {
                EventID = 3,
                Organization = "10",
                Description = "Business Summit",
                Account = "BIZSUMMIT",
                StartDate = new DateTime(2026, 9, 10, 8, 0, 0),
                EndDate = new DateTime(2026, 9, 11, 17, 0, 0),
                StartTime = new DateTime(2026, 9, 10, 8, 0, 0),
                EndTime = new DateTime(2026, 9, 11, 17, 0, 0),
                Status = "30",
                Type = "EDU",
                Category = "BU",
                Attendance = 200,
                ForecastAttendance = 200,
                ForecastRevenue = 99998,
                Description1 = "Executive business summit for industry leaders",
                Description2 = "New York, NY",
                Coordinator = "BIZCOORD",
                Contact = "BIZCONT"
            }
        };
    }
}

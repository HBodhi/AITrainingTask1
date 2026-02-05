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

    public async Task<Event?> GetEventByIdAsync(string id)
    {
        try
        {
            _logger.LogInformation("Fetching event {EventId} from Momentus API", id);
            
            // For now, returning mock data
            // In production: var response = await _httpClient.GetFromJsonAsync<Event>($"{_momentusApiBaseUrl}/events/{id}");
            
            var events = GetMockEvents();
            return await Task.FromResult(events.FirstOrDefault(e => e.Id == id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching event {EventId} from Momentus API", id);
            throw;
        }
    }

    private IEnumerable<Event> GetMockEvents()
    {
        // Mock data simulating Momentus event data
        return new List<Event>
        {
            new Event
            {
                Id = "1",
                Name = "Tech Conference 2026",
                Description = "Annual technology conference featuring the latest innovations",
                StartDate = new DateTime(2026, 6, 15, 9, 0, 0),
                EndDate = new DateTime(2026, 6, 17, 18, 0, 0),
                Location = "San Francisco, CA",
                Organizer = "Tech Events Inc",
                Capacity = 500,
                Price = 299.99m
            },
            new Event
            {
                Id = "2",
                Name = "Music Festival 2026",
                Description = "Three-day outdoor music festival with top artists",
                StartDate = new DateTime(2026, 7, 20, 12, 0, 0),
                EndDate = new DateTime(2026, 7, 22, 23, 0, 0),
                Location = "Austin, TX",
                Organizer = "Music Events Co",
                Capacity = 10000,
                Price = 149.99m
            },
            new Event
            {
                Id = "3",
                Name = "Business Summit",
                Description = "Executive business summit for industry leaders",
                StartDate = new DateTime(2026, 9, 10, 8, 0, 0),
                EndDate = new DateTime(2026, 9, 11, 17, 0, 0),
                Location = "New York, NY",
                Organizer = "Business Networks",
                Capacity = 200,
                Price = 499.99m
            }
        };
    }
}

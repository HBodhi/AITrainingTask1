using MomentusEventApi.Models;
using Ungerboeck.Api.Models.Search;

namespace MomentusEventApi.Services;

public class MomentusEventService : IMomentusEventService
{
    private readonly IUngerboeckApiClientFactory _clientFactory;
    private readonly ILogger<MomentusEventService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _defaultOrganization;
    private readonly bool _useMockData;

    public MomentusEventService(
        IUngerboeckApiClientFactory clientFactory, 
        ILogger<MomentusEventService> logger, 
        IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _configuration = configuration;
        _defaultOrganization = configuration["UngerboeckApi:DefaultOrganization"] ?? "10";
        
        // Check if we should use mock data (for testing without real API credentials)
        _useMockData = configuration.GetValue<bool>("UngerboeckApi:UseMockData", false);
        
        if (_useMockData)
        {
            _logger.LogWarning("Service is configured to use MOCK DATA. Set UngerboeckApi:UseMockData to false to use real API.");
        }
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all events from Ungerboeck API for organization {Organization}", _defaultOrganization);
            
            if (_useMockData)
            {
                _logger.LogDebug("Returning mock data");
                return GetMockEvents();
            }

            // Use the SDK to fetch events
            var client = _clientFactory.CreateClient();
            
            // Search for all events in the organization
            // Using a simple search that returns recent events
            var searchResponse = await Task.Run(() => 
                client.Endpoints.Events.Search(_defaultOrganization, ""));
            
            var resultCount = searchResponse?.Results != null ? searchResponse.Results.Count() : 0;
            _logger.LogInformation("Retrieved {Count} events from Ungerboeck API", resultCount);
            
            if (searchResponse == null || searchResponse.Results == null)
            {
                _logger.LogWarning("No events found or null response from Ungerboeck API");
                return new List<Event>();
            }
            
            // Convert EventsModel to Event (our wrapper class)
            return searchResponse.Results.Select(e => new Event
            {
                EventID = e.EventID,
                Organization = e.Organization,
                Description = e.Description,
                Account = e.Account,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Status = e.Status,
                Type = e.Type,
                Category = e.Category,
                Attendance = e.Attendance,
                ForecastAttendance = e.ForecastAttendance,
                ForecastRevenue = e.ForecastRevenue,
                Description1 = e.Description1,
                Description2 = e.Description2,
                Coordinator = e.Coordinator,
                Contact = e.Contact,
                Class = e.Class,
                Salesperson = e.Salesperson,
                ActualRevenue = e.ActualRevenue,
                OrderedRevenue = e.OrderedRevenue,
                RevisedRevenue = e.RevisedRevenue
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching events from Ungerboeck API");
            throw;
        }
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching event {EventId} from Ungerboeck API for organization {Organization}", 
                id, _defaultOrganization);
            
            if (_useMockData)
            {
                _logger.LogDebug("Returning mock data");
                var events = GetMockEvents();
                return await Task.FromResult(events.FirstOrDefault(e => e.EventID == id));
            }

            // Use the SDK to fetch a specific event
            var client = _clientFactory.CreateClient();
            
            var eventModel = await Task.Run(() => 
                client.Endpoints.Events.Get(_defaultOrganization, id));
            
            if (eventModel == null)
            {
                _logger.LogWarning("Event {EventId} not found in organization {Organization}", id, _defaultOrganization);
                return null;
            }

            _logger.LogInformation("Retrieved event {EventId}: {Description}", id, eventModel.Description);
            
            // Convert EventsModel to Event
            return new Event
            {
                EventID = eventModel.EventID,
                Organization = eventModel.Organization,
                Description = eventModel.Description,
                Account = eventModel.Account,
                StartDate = eventModel.StartDate,
                EndDate = eventModel.EndDate,
                StartTime = eventModel.StartTime,
                EndTime = eventModel.EndTime,
                Status = eventModel.Status,
                Type = eventModel.Type,
                Category = eventModel.Category,
                Attendance = eventModel.Attendance,
                ForecastAttendance = eventModel.ForecastAttendance,
                ForecastRevenue = eventModel.ForecastRevenue,
                Description1 = eventModel.Description1,
                Description2 = eventModel.Description2,
                Coordinator = eventModel.Coordinator,
                Contact = eventModel.Contact,
                Class = eventModel.Class,
                Salesperson = eventModel.Salesperson,
                ActualRevenue = eventModel.ActualRevenue,
                OrderedRevenue = eventModel.OrderedRevenue,
                RevisedRevenue = eventModel.RevisedRevenue,
                // Include additional commonly used fields
                ParentEvent = eventModel.ParentEvent,
                PreviousEvent = eventModel.PreviousEvent,
                AlternateEvent = eventModel.AlternateEvent,
                Abbreviation = eventModel.Abbreviation,
                LegalName = eventModel.LegalName,
                WebAddress = eventModel.WebAddress,
                Public = eventModel.Public,
                BoxOffice = eventModel.BoxOffice,
                InDate = eventModel.InDate,
                OutDate = eventModel.OutDate,
                InTime = eventModel.InTime,
                OutTime = eventModel.OutTime,
                EventUserFieldSets = eventModel.EventUserFieldSets
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching event {EventId} from Ungerboeck API", id);
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

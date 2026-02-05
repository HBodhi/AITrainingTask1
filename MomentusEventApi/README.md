# Momentus Event API

A .NET 10 Web API that fetches event details from Momentus (formerly Ungerboeck) using the official Ungerboeck.Api.SDK.

## Overview

This API provides endpoints to retrieve event information from the Momentus/Ungerboeck platform. It uses the official `Ungerboeck.Api.Sdk` package to interact with the Ungerboeck API, providing full access to event management capabilities.

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- **Ungerboeck.Api.Sdk (v1.253.1.4)** - Official SDK for API interaction
- Ungerboeck.Api.Models (v1.253.1.4) - Model definitions
- OpenAPI/Swagger support
- JWT Authentication for Ungerboeck API

## Project Structure

```
MomentusEventApi/
├── Controllers/
│   └── EventsController.cs           # API endpoints
├── Models/
│   └── Event.cs                       # Event model (extends EventsModel)
├── Services/
│   ├── IMomentusEventService.cs      # Service interface
│   ├── MomentusEventService.cs       # Service implementation
│   ├── IUngerboeckApiClientFactory.cs # Client factory interface
│   └── UngerboeckApiClientFactory.cs  # Client factory implementation
├── Program.cs                          # Application entry point
├── appsettings.json                    # Configuration
└── appsettings.Development.json        # Development configuration
```

## Event Model

The Event model inherits from `Ungerboeck.Api.Models.Subjects.EventsModel`, providing full compatibility with the Momentus/Ungerboeck API. Key properties include:

- **EventID** (int?): Unique event identifier
- **Organization** (string): Organization code
- **Description** (string): Event name/description
- **StartDate/EndDate** (DateTime?): Event dates
- **StartTime/EndTime** (DateTime?): Event times
- **Account** (string): Account code
- **Status** (string): Event status code
- **Type** (string): Event type code
- **Category** (string): Event category code
- **Attendance** (int?): Expected attendance
- **ForecastRevenue** (int?): Forecasted revenue
- **Coordinator** (string): Coordinator account code
- **Contact** (string): Contact account code
- And many more properties as defined in the Ungerboeck.Api.Models package

For a complete list of properties, refer to the [Ungerboeck.Api.Models NuGet package](https://www.nuget.org/packages/Ungerboeck.Api.Models/).

## API Endpoints

### Get All Events
```
GET /api/events
```
Returns a list of all events from Momentus.

**Response:** 200 OK
```json
[
  {
    "eventID": 1,
    "organization": "10",
    "description": "Tech Conference 2026",
    "startDate": "2026-06-15T09:00:00",
    "endDate": "2026-06-17T18:00:00",
    "startTime": "2026-06-15T09:00:00",
    "endTime": "2026-06-17T18:00:00",
    "status": "30",
    "type": "EDU",
    "category": "CO",
    "account": "TECHCONF",
    "attendance": 500,
    "forecastAttendance": 500,
    "forecastRevenue": 299990,
    "description1": "Annual technology conference featuring the latest innovations",
    "description2": "San Francisco, CA",
    "coordinator": "TECHCOORD",
    "contact": "TECHCONT"
  }
]
```

### Get Event by ID
```
GET /api/events/{id}
```
Returns details for a specific event.

**Parameters:**
- `id` (int): The event ID (integer)

**Response:** 200 OK
```json
{
  "eventID": 1,
  "organization": "10",
  "description": "Tech Conference 2026",
  "startDate": "2026-06-15T09:00:00",
  "endDate": "2026-06-17T18:00:00",
  "startTime": "2026-06-15T09:00:00",
  "endTime": "2026-06-17T18:00:00",
  "status": "30",
  "type": "EDU",
  "category": "CO",
  "account": "TECHCONF",
  "attendance": 500,
  "forecastAttendance": 500,
  "forecastRevenue": 299990,
  "description1": "Annual technology conference featuring the latest innovations",
  "description2": "San Francisco, CA",
  "coordinator": "TECHCOORD",
  "contact": "TECHCONT"
}
```

### Search Events
```
GET /api/events/search?filter={searchFilter}
```
Search for events using Ungerboeck OData-style search filter syntax.

**Parameters:**
- `filter` (string, optional): OData-style search filter

**Filter Examples:**
- `Description eq 'Tech Conference'` - Exact match
- `Status eq '30'` - Search by status (30 = Firm)
- `StartDate gt 2026-01-01` - Events starting after a date
- Leave empty to return all events

**Response:** 200 OK
```json
[
  {
    "eventID": 1,
    "organization": "10",
    "description": "Tech Conference 2026",
    ...
  }
]
```

## Configuration

### Ungerboeck API Setup

Before using the API with real Ungerboeck data, you need to configure the connection. Update `appsettings.json`:

```json
{
  "UngerboeckApi": {
    "BaseUrl": "https://your-site.ungerboeck.com",
    "ApiUserId": "YOUR_API_USER_ID",
    "Secret": "your-secret-guid",
    "Key": "your-key-guid",
    "DefaultOrganization": "10"
  }
}
```

#### Getting API Credentials

1. **Log in to Ungerboeck/Momentus**
2. **Navigate to Main Menu → API Users**
3. **Create or select an API User**
4. **Get the credentials:**
   - **API User ID**: Found in the API User details
   - **Secret**: GUID found in the API User details
   - **Key**: GUID from the Keys section (any one of the keys)
   - **BaseUrl**: Your Ungerboeck site URL

### Development Mode (Mock Data)

For development and testing without Ungerboeck credentials, the API can use mock data. This is enabled by default in `appsettings.Development.json`:

```json
{
  "UngerboeckApi": {
    "UseMockData": true,
    ...
  }
}
```

Set `UseMockData` to `false` to use the real Ungerboeck API.

## Running the Application

### Prerequisites
- .NET 10 SDK installed

### Build and Run

1. Navigate to the project directory:
```bash
cd MomentusEventApi
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5001`
- HTTPS: `https://localhost:7037`

### Access OpenAPI Documentation

When running in development mode, you can access the OpenAPI specification at:
```
http://localhost:5001/openapi/v1.json
```

## Testing

The API can be tested using:
- Browser (for GET requests)
- Postman or similar API testing tools
- The included `MomentusEventApi.http` file with REST Client extensions
- curl commands:

```bash
# Get all events
curl http://localhost:5001/api/events

# Get specific event
curl http://localhost:5001/api/events/1

# Search events
curl "http://localhost:5001/api/events/search?filter=Conference"
```

## Package Version Compatibility

This project uses **Ungerboeck.Api.Sdk v1.253.1.4**, which is compatible with Momentus/Ungerboeck version 25.3.

The version number format is `1.XXX.Y.Z` where:
- **XXX** represents the Ungerboeck/Momentus version (253 = version 25.3)
- **Y.Z** represents the package build number

**Important:** Always match your SDK package version to your Momentus/Ungerboeck environment version:
- Version 1.231.x.x → Ungerboeck 23.1
- Version 1.241.x.x → Ungerboeck 24.1  
- Version 1.253.x.x → Ungerboeck 25.3 (Current)

To update the package version:
```bash
dotnet add package Ungerboeck.Api.Sdk --version [your-version]
dotnet add package Ungerboeck.Api.Models --version [your-version]
```

## Development

### Using Mock Data for Development

By default in development mode, the API uses mock data so you can develop and test without Ungerboeck credentials:

1. Leave `UseMockData: true` in `appsettings.Development.json`
2. The API will return sample event data
3. All endpoints work normally with mock data

### Connecting to Real Ungerboeck API

To use the real Ungerboeck API:

1. Update `appsettings.json` or `appsettings.Production.json` with your credentials
2. Set `UseMockData: false`  
3. Ensure your Ungerboeck API User has appropriate permissions
4. Test connection with a simple GET request

### Adding New Endpoints

1. Add new methods to the `IMomentusEventService` interface
2. Implement the methods in `MomentusEventService` using the SDK client
3. Add corresponding controller actions in `EventsController`
4. Update the `.http` file with examples

### Example: Adding a New Method

```csharp
// 1. Add to IMomentusEventService
Task<Event> AddEventAsync(Event newEvent);

// 2. Implement in MomentusEventService
public async Task<Event> AddEventAsync(Event newEvent)
{
    var client = _clientFactory.CreateClient();
    var result = await Task.Run(() => 
        client.Endpoints.Events.Add(newEvent));
    return result;
}

// 3. Add controller action
[HttpPost]
public async Task<ActionResult<Event>> CreateEvent([FromBody] Event newEvent)
{
    var created = await _eventService.AddEventAsync(newEvent);
    return CreatedAtAction(nameof(GetEventById), 
        new { id = created.EventID }, created);
}
```

## Troubleshooting

### Authentication Errors

If you receive authentication errors:
1. Verify your API User credentials in Ungerboeck
2. Ensure the API User is active
3. Check that the Secret and Key GUIDs are correct
4. Confirm your BaseUrl includes the correct domain

### No Events Returned

If searches return no events:
1. Check the DefaultOrganization setting matches your org code
2. Verify the API User has permissions to view events
3. Try searching without a filter first
4. Check Ungerboeck logs for API access attempts

## Future Enhancements

- Implement event creation/updates
- Add function and order endpoints
- Implement pagination for large result sets
- Add caching for improved performance
- Add comprehensive unit and integration tests
- Implement batch operations
- Add webhook support for real-time updates

## Additional Resources

- [Ungerboeck API Documentation](https://supportcenter.ungerboeck.com/hc/en-us/sections/115001365327-API-Basics)
- [Ungerboeck SDK Examples](https://github.com/UngerboeckAPI/253)
- [NuGet Package](https://www.nuget.org/packages/Ungerboeck.Api.Sdk/)

## License

This project is part of the AITrainingTask1 repository.

```bash
# Get all events
curl http://localhost:5001/api/events

# Get specific event
curl http://localhost:5001/api/events/1

# Search events
curl "http://localhost:5001/api/events/search?filter=Conference"
```

## License

This project is part of the AITrainingTask1 repository.

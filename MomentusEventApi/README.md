# Momentus Event API

A .NET 10 Web API that fetches event details from Momentus (formerly Ungerboeck).

## Overview

This API provides endpoints to retrieve event information from the Momentus platform. It includes endpoints to fetch all events or specific events by ID. The API uses the official `Ungerboeck.Api.Models` NuGet package for event model definitions.

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Ungerboeck.Api.Models (v1.253.1.4)
- OpenAPI/Swagger support

## Project Structure

```
MomentusEventApi/
├── Controllers/
│   └── EventsController.cs      # API endpoints
├── Models/
│   └── Event.cs                  # Event model (extends EventsModel from Ungerboeck.Api.Models)
├── Services/
│   ├── IMomentusEventService.cs  # Service interface
│   └── MomentusEventService.cs   # Service implementation
├── Program.cs                     # Application entry point
└── appsettings.json              # Configuration
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
- `id` (string): The event ID

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

## Configuration

The Momentus API configuration can be found in `appsettings.json`:

```json
{
  "MomentusApi": {
    "BaseUrl": "https://api.momentus.com",
    "ApiKey": ""
  }
}
```

**Note:** The current implementation uses mock data for demonstration purposes. To connect to the actual Momentus API:
1. Add your Momentus API key to the configuration
2. Update the `MomentusEventService.cs` to uncomment the actual API calls
3. Ensure you're using the correct version of Ungerboeck.Api.Models that matches your Momentus/Ungerboeck system version

### Ungerboeck.Api.Models Version Compatibility

This project uses **Ungerboeck.Api.Models v1.253.1.4**, which is compatible with Momentus/Ungerboeck version 25.3. The version number format is `1.XXX.Y.Z` where:
- **XXX** represents the Ungerboeck/Momentus version (253 = version 25.3)
- **Y.Z** represents the package build number

**Important:** Always match your model package version to your Momentus/Ungerboeck API environment version to ensure compatibility. For example:
- Version 1.231.x.x → Ungerboeck 23.1
- Version 1.241.x.x → Ungerboeck 24.1  
- Version 1.253.x.x → Ungerboeck 25.3

To update the package version:
```bash
dotnet add package Ungerboeck.Api.Models --version [your-version]
```

## Development

### Adding New Endpoints

1. Add new methods to the `IMomentusEventService` interface
2. Implement the methods in `MomentusEventService`
3. Add corresponding controller actions in `EventsController`

### Testing

The API can be tested using:
- Browser (for GET requests)
- Postman or similar API testing tools
- curl commands:

```bash
# Get all events
curl http://localhost:5001/api/events

# Get specific event
curl http://localhost:5001/api/events/1
```

## Future Enhancements

- Implement actual Momentus API integration
- Add authentication/authorization
- Implement caching for improved performance
- Add filtering, sorting, and pagination
- Add unit and integration tests
- Add search functionality
- Implement event creation/updates (if supported by Momentus API)

## License

This project is part of the AITrainingTask1 repository.

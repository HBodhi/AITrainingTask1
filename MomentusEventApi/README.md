# Momentus Event API

A .NET 10 Web API that fetches event details from Momentus.

## Overview

This API provides endpoints to retrieve event information from the Momentus platform. It includes endpoints to fetch all events or specific events by ID.

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- OpenAPI/Swagger support

## Project Structure

```
MomentusEventApi/
├── Controllers/
│   └── EventsController.cs      # API endpoints
├── Models/
│   └── Event.cs                  # Event model
├── Services/
│   ├── IMomentusEventService.cs  # Service interface
│   └── MomentusEventService.cs   # Service implementation
├── Program.cs                     # Application entry point
└── appsettings.json              # Configuration
```

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
    "id": "1",
    "name": "Tech Conference 2026",
    "description": "Annual technology conference featuring the latest innovations",
    "startDate": "2026-06-15T09:00:00",
    "endDate": "2026-06-17T18:00:00",
    "location": "San Francisco, CA",
    "organizer": "Tech Events Inc",
    "capacity": 500,
    "price": 299.99
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
  "id": "1",
  "name": "Tech Conference 2026",
  "description": "Annual technology conference featuring the latest innovations",
  "startDate": "2026-06-15T09:00:00",
  "endDate": "2026-06-17T18:00:00",
  "location": "San Francisco, CA",
  "organizer": "Tech Events Inc",
  "capacity": 500,
  "price": 299.99
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
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Access OpenAPI Documentation

When running in development mode, you can access the OpenAPI specification at:
```
http://localhost:5000/openapi/v1.json
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
3. Replace the mock data methods with real API integration

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
curl http://localhost:5000/api/events

# Get specific event
curl http://localhost:5000/api/events/1
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

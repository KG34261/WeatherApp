# Weather API Service

This application provides weather data and daily recommendations based on coordinates (latitude and longitude). It fetches real-time weather information from the OpenWeatherMap API and returns structured responses for client consumption.

## Features

- Validates latitude and longitude coordinates.
- Retrieves weather data asynchronously from OpenWeatherMap.
- Handles system failures and downstream service errors gracefully.
- Provides appropriate HTTP status codes for client feedback.
- Logs errors for monitoring and troubleshooting.

## API Endpoint

### GET `/weather?latitude={lat}&longitude={lon}`

- **Parameters:**
  - `latitude` (double) — Latitude coordinate.
  - `longitude` (double) — Longitude coordinate.

- **Responses:**
  - `200 OK` — Returns weather data and recommendations.
  - `400 Bad Request` — Invalid coordinates or downstream service rejected request.
  - `404 Not Found` — Weather data not found for the specified location.
  - `503 Service Unavailable` — Temporary system failure or downstream service unavailable.
  - `500 Internal Server Error` — Unexpected server error.

## Setup Instructions

1. Clone the repository and open the solution in your IDE.

2. Obtain an API key from [OpenWeatherMap](https://openweathermap.org/api).

3. Add your OpenWeatherMap API key to the configuration file (e.g., `appsettings.json`) or environment variables. Example:

    ```json
    {
      "WeatherApi": {
        "ApiKey": "YOUR_API_KEY_HERE"
      }
    }
    ```

4. Ensure the service and HTTP client dependencies are properly configured to use the API key.

5. Run the application.

## Dependencies

- .NET 8 or later
- HttpClient for API calls
- Logging framework (e.g., Microsoft.Extensions.Logging)
- OpenWeatherMap API

## Error Handling

- The app simulates occasional system failures for testing purposes.
- Downstream errors (like API unavailability) are caught and logged, returning appropriate HTTP status codes.
- Unexpected exceptions return a generic 500 error with logging.

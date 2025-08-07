using Microsoft.AspNetCore.Mvc;
using WeatherApp.Helpers;
using WeatherApp.Interface.Helpers;
using WeatherApp.Interface.Service;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private static int _requestCount = 0;
        private static readonly object _lock = new();
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;
        private readonly IRequestCounter _requestCounter;
        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService, IRequestCounter requestCounter)
        {
            _logger = logger;
            _weatherService = weatherService;
            _requestCounter = requestCounter;
        }
        /// <summary>
        /// Retrieves current weather information based on provided latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GetWeather(double latitude, double longitude)
        {
            if (_requestCounter.ShouldFail())
            {
                return StatusCode(503, "System failure");
            }
            if (!LatLongHelper.IsValidCoordinates(latitude, longitude))
            {
                return BadRequest("Invalid coordinate values.");
            }
            try
            {
                var result = await _weatherService.GetWeather(latitude, longitude);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
            {
                _logger.LogError(ex, "Downstream service error");
                int statusCode = (int)ex.StatusCode.Value;

                if (statusCode >= 500)
                {
                    return StatusCode(503, "Downstream service unavailable.");
                }

                return BadRequest("Downstream service rejected the request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetWeather");
                return StatusCode(500, "Internal server error.");
            }

        }


    }
}

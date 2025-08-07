using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Configurations;
using WeatherApp.Interface.Repository;
using WeatherApp.Models;

namespace WeatherApp.Repository
{

    public class WeatherRepository : IWeatherRepository
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiOptions _options;
        public WeatherRepository(HttpClient httpClient, IOptions<WeatherApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        /// <summary>
        /// Gets Weather information from the OpenWeatherMap API
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<OpenWeatherMap> GetWeather(double latitude, double longitude)
        {
            var url = $"{_options.BaseUrl}?lat={latitude}&lon={longitude}&appid={_options.ApiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadFromJsonAsync<OpenWeatherMap>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return json;
        }
    }
}

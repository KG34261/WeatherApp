using WeatherApp.Models;

namespace WeatherApp.Interface.Service
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeather(double latitude, double longitude);
    }
}

using WeatherApp.Models;

namespace WeatherApp.Interface.Repository
{
    public interface IWeatherRepository
    {
        Task<OpenWeatherMap> GetWeather(double latitude, double longitude);
    }
}

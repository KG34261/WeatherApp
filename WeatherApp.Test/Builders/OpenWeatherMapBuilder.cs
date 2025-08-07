using System.Collections.Generic;
using WeatherApp.Models;

namespace WeatherApp.Tests.Builders
{
    public static class OpenWeatherMapBuilder
    {
        public static OpenWeatherMap Build(
            double temp = 9.45,
            double windSpeed = 15.0,
            int windDeg = 180,
            double windGust = 7.0,
            int weatherId = 800,
            string main = "Clear",
            string description = "clear sky",
            string icon = "01d")
        {
            return new OpenWeatherMap
            {
                Main = new MainInfo
                {
                    Temp = temp
                },
                Wind = new WindInfo
                {
                    Speed = windSpeed,
                    Deg = windDeg,
                    Gust = windGust
                },
                Weather = new List<WeatherInfo>
                {
                    new WeatherInfo
                    {
                        Id = weatherId,
                        Main = main,
                        Description = description,
                        Icon = icon
                    }
                }
            };
        }
    }
}

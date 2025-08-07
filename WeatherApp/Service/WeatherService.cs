using WeatherApp.Interface.Repository;
using WeatherApp.Interface.Service;
using WeatherApp.Models;

namespace WeatherApp.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }
        /// <summary>
        /// Gets Weather based on Latitude and Longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<WeatherResponse> GetWeather(double latitude, double longitude)
        {
            var response = await _weatherRepository.GetWeather(latitude, longitude);
            var weatherCondition = GetWeatherCondition(response.Weather.FirstOrDefault().Id,response.Wind.Speed);
            var result = new WeatherResponse {
                Temperature = response.Main.Temp,
                WindSpeed = response.Wind.Speed,
                WeatherCondition = weatherCondition.ToString(),
                Recommendations = GetAdvice(weatherCondition, response.Main.Temp)
            };
            return result;
        }
        /// <summary>
        /// Produces Advice based on the weather information available
        /// </summary>
        /// <param name="weather"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        private List<string> GetAdvice(WeatherCondition weather, double temperature)
        {
            var advice = new List<string>();

            if (weather == WeatherCondition.Sunny)
            {
                advice.Add("Don't forget to bring a hat");
            }
            if (temperature > 25)
            {
                advice.Add("It's a great day for a swim");
            }
            if (temperature < 15 && (weather == WeatherCondition.Rainy || weather == WeatherCondition.Snowing))
            {
                advice.Add("Don't forget to bring a coat");
            }
            if (weather == WeatherCondition.Rainy)
            {
                advice.Add("Don't forget the umbrella");
            }
            return advice;
        }

        /// <summary>
        /// Produces basic weather condition based on weather id from OpenWeatherMap API
        /// </summary>
        /// <param name="weatherId"></param>
        /// <param name="WindSpeed"></param>
        /// <returns></returns>
        private WeatherCondition GetWeatherCondition(int weatherId, double WindSpeed)
        {
            if(WindSpeed >= 30)
            {
                return WeatherCondition.Windy;
            }
            // Thunderstorm as Windy
            if (weatherId >= 200 && weatherId < 300)
                return WeatherCondition.Windy; 
            //drizzle and rain considered Rainy
            if ((weatherId >= 300 && weatherId < 400) || (weatherId >= 500 && weatherId < 600)) 
                return WeatherCondition.Rainy;

            if (weatherId >= 600 && weatherId < 700)
                return WeatherCondition.Snowing;

            if (weatherId >= 700 && weatherId < 800)
                return WeatherCondition.Windy; // e.g., Mist, Fog, Smoke, etc.

            if (weatherId >= 800 && weatherId <= 804)
                return WeatherCondition.Sunny; //

            //it is Wellington after all! :) 
            return WeatherCondition.Windy;

        }


    }
}

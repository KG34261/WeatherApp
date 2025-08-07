using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interface.Repository;
using WeatherApp.Models;
using WeatherApp.Service;
using WeatherApp.Tests.Builders;

namespace WeatherApp.Test
{
    public class WeatherServiceTest
    {

        [Fact]
        public async Task GetWeather_ReturnsCorrectRecommendations_ForSunnyHotWeather()
        {
            // Arrange
            var weatherData = OpenWeatherMapBuilder.Build(temp: 30.0, windSpeed: 5, weatherId: 800);

            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);
            var service = new WeatherService(mockRepo.Object);
            // Act
            var result = await service.GetWeather(0, 0);

            // Assert
            Assert.Equal(30, result.Temperature);
            Assert.Equal(5, result.WindSpeed);
            Assert.Equal("Sunny", result.WeatherCondition);
            Assert.Contains("Don't forget to bring a hat", result.Recommendations);
            Assert.Contains("It's a great day for a swim", result.Recommendations);
            Assert.Equal(2, result.Recommendations.Count);
        }
        [Fact]
        public async Task GetWeather_ReturnsCorrectRecommendations_ForRainyColdWeather()
        {
            // Arrange
            var weatherData = OpenWeatherMapBuilder.Build(temp: 5.0, windSpeed: 15, weatherId: 501);

            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);

            var service = new WeatherService(mockRepo.Object);

            // Act
            var result = await service.GetWeather(0, 0);

            // Assert
            Assert.Equal(5.0, result.Temperature);
            Assert.Equal(15, result.WindSpeed);
            Assert.Equal(WeatherCondition.Rainy.ToString(), result.WeatherCondition);
            Assert.Contains("Don't forget to bring a coat", result.Recommendations);
            Assert.Contains("Don't forget the umbrella", result.Recommendations);
            Assert.Equal(2, result.Recommendations.Count);
        }
        [Theory]
        [InlineData(600)]
        [InlineData(605)]
        public async Task GetWeather_SnowySet(int weatherId)
        {
            //Arrange
            var weatherData = OpenWeatherMapBuilder.Build(weatherId:weatherId);
            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);

            var service = new WeatherService(mockRepo.Object);

            // Act
            var result = await service.GetWeather(0, 0);
            //Assert
            Assert.Equal(WeatherCondition.Snowing.ToString(), result.WeatherCondition);
        }
        [Theory]
        [InlineData(800)]
        [InlineData(804)]

        public async Task GetWeather_SunnySet(int weatherId)
        {
            //Arrange
            var weatherData = OpenWeatherMapBuilder.Build(weatherId: weatherId);
            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);

            var service = new WeatherService(mockRepo.Object);

            // Act
            var result = await service.GetWeather(0, 0);
            //Assert
            Assert.Equal(WeatherCondition.Sunny.ToString(), result.WeatherCondition);
        }
        [Theory]
        [InlineData(800,30)]
        [InlineData(200,45)]
        [InlineData(301,37)]

        public async Task GetWeather_WindySet(int weatherId,double windSpeed)
        {
            //Arrange
            var weatherData = OpenWeatherMapBuilder.Build(windSpeed:windSpeed, weatherId:weatherId);
            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);

            var service = new WeatherService(mockRepo.Object);

            // Act
            var result = await service.GetWeather(0, 0);
            //Assert
            Assert.Equal(WeatherCondition.Windy.ToString(), result.WeatherCondition);
        }
        [Theory]
        [InlineData(500)]
        [InlineData(520)]
        [InlineData(531)]
        public async Task GetWeather_RainySet(int weatherId)
        {
            //Arrange
            var weatherData = OpenWeatherMapBuilder.Build( weatherId: weatherId);
            var mockRepo = new Mock<IWeatherRepository>();
            mockRepo.Setup(r => r.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                    .ReturnsAsync(weatherData);

            var service = new WeatherService(mockRepo.Object);

            // Act
            var result = await service.GetWeather(0, 0);
            //Assert
            Assert.Equal(WeatherCondition.Rainy.ToString(), result.WeatherCondition);
        }



    }
}

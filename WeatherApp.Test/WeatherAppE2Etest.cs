using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Configurations;
using WeatherApp.Controllers;
using WeatherApp.Helpers;
using WeatherApp.Interface.Helpers;
using WeatherApp.Models;
using WeatherApp.Repository;
using WeatherApp.Service;
using WeatherApp.Test.Builders;
using WeatherApp.Tests.Builders;

namespace WeatherApp.Test
{
    public class WeatherAppE2Etest
    {
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<IOptions<WeatherApiOptions>> _optionsMock;
        private Mock<IRequestCounter> _mockRequestCounter;


        public WeatherAppE2Etest()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _mockLogger = new Mock<ILogger<WeatherController>>();
            _mockRequestCounter = new Mock<IRequestCounter>();
            _mockRequestCounter.Setup(x => x.ShouldFail()).Returns(false);
            _optionsMock = new Mock<IOptions<WeatherApiOptions>>();
            _optionsMock.Setup(o => o.Value).Returns(new WeatherApiOptions
            {
                BaseUrl = "https://fake-weather.com"
            });
        }
        [Fact]
        public async Task GetWeather_ReturnsOk()
        {
            //arrange
            var jsonstring = JsonSerializer.Serialize(OpenWeatherMapBuilder.Build());
            var httpclient = HttpClientBuilder.CreateClient(HttpStatusCode.OK, jsonstring, out _);

            var repository = new WeatherRepository(httpclient, _optionsMock.Object);
            var service = new WeatherService(repository);
            var controller = new WeatherController(_mockLogger.Object,service,_mockRequestCounter.Object);
            var result =  await controller.GetWeather(1.0, 2.0);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var weatherResponse = Assert.IsType<WeatherResponse>(okResult.Value);

            Assert.Equal(9.45, weatherResponse.Temperature);
            Assert.Equal(15, weatherResponse.WindSpeed);
            Assert.Equal("Sunny", weatherResponse.WeatherCondition);
            Assert.Equal("Metric", weatherResponse.MeasureUnits);
            Assert.Single(weatherResponse.Recommendations);
            Assert.Equal("Don't forget to bring a hat", weatherResponse.Recommendations[0]);
        }
        [Fact]
        public async Task GetWeather_BadRequest()
        {
            var httpclient = HttpClientBuilder.CreateClient(HttpStatusCode.BadRequest, "", out _);
            var repository = new WeatherRepository(httpclient, _optionsMock.Object);
            var service = new WeatherService(repository);
            var controller = new WeatherController(_mockLogger.Object, service, _mockRequestCounter.Object);
            var result = await controller.GetWeather(1.0, 2.0);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Downstream service rejected the request.", badRequestResult.Value);
        }
        [Fact]
        public async Task GetWeather_ServiceUnavailable()
        {
            // Arrange
            var httpclient = HttpClientBuilder.CreateClient(
                HttpStatusCode.ServiceUnavailable,
                string.Empty,
                out _
            );

            var repository = new WeatherRepository(httpclient, _optionsMock.Object);
            var service = new WeatherService(repository);
            var controller = new WeatherController(_mockLogger.Object, service, _mockRequestCounter.Object);

            // Act
            var result = await controller.GetWeather(1.0, 2.0);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusResult.StatusCode);
            Assert.Equal("Downstream service unavailable.", statusResult.Value);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Controllers;
using WeatherApp.Helpers;
using WeatherApp.Interface.Helpers;
using WeatherApp.Interface.Service;
using WeatherApp.Models;

namespace WeatherApp.Test
{
    public class WeatherControllerTest
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private readonly WeatherController _controller;
        private readonly Mock<IRequestCounter> _mockRequestCounter;

        public WeatherControllerTest()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();
            _mockRequestCounter = new Mock<IRequestCounter>();
            _mockRequestCounter.Setup(x => x.ShouldFail()).Returns(false); 
            _controller = new WeatherController(_mockLogger.Object, _mockWeatherService.Object, _mockRequestCounter.Object);
        }

        [Fact]
        public async Task GetWeather_ShouldReturn503_WhenRequestCounterIndicatesFailure()
        {

            _mockRequestCounter.Setup(x=>x.ShouldFail()).Returns(true);///overide default
            // Act
            var result = await _controller.GetWeather(10, 10);
            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusResult.StatusCode);
            Assert.Equal("System failure", statusResult.Value);

            // Reset fail flag if needed
        }

        [Fact]
        public async Task GetWeather_ShouldReturn503_WhenDownstreamThrowsHttpRequestExceptionWith5xx()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ThrowsAsync(new HttpRequestException("Downstream 500 error", null, HttpStatusCode.InternalServerError));

            // Act
            var result = await _controller.GetWeather(10, 10);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusResult.StatusCode);
            Assert.Equal("Downstream service unavailable.", statusResult.Value);
        }

        [Fact]
        public async Task GetWeather_ShouldReturnBadRequest_WhenDownstreamThrowsHttpRequestExceptionWith4xx()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ThrowsAsync(new HttpRequestException("Downstream 400 error", null, HttpStatusCode.BadRequest));

            // Act
            var result = await _controller.GetWeather(10, 10);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Downstream service rejected the request.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetWeather_ShouldReturnBadRequest_WhenCoordinatesInvalid()
        {
            // Act
            var result = await _controller.GetWeather(999, 999);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid coordinate values.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetWeather_ShouldReturnOk_WhenServiceReturnsData()
        {
            // Arrange
            var dummyWeather = new WeatherResponse { Temperature = 20, WeatherCondition = "Sunny" };
            _mockWeatherService.Setup(s => s.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(dummyWeather);

            // Act
            var result = await _controller.GetWeather(10, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dummyWeather, okResult.Value);
        }

    }

}

using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Configurations;
using WeatherApp.Models;
using WeatherApp.Repository;
using WeatherApp.Test.Builders;
using WeatherApp.Tests.Builders;
using Xunit;

namespace WeatherApp.Test
{
    public class WeatherRepositoryTests
    {
        private readonly IOptions<WeatherApiOptions> _options;

        public WeatherRepositoryTests()
        {
            _options = Options.Create(new WeatherApiOptions
            {
                BaseUrl = "https://FakeOpenWeather.com/weather",
                ApiKey = "testingkey"
            });
        }
        [Fact]
        public async Task GetWeather_ReturnsDeserializedResult_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expected = OpenWeatherMapBuilder.Build();


            var jsonResponse = JsonSerializer.Serialize(expected);
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = HttpClientBuilder.CreateClient(HttpStatusCode.OK, jsonResponse, out _);

            var repo = new WeatherRepository(httpClient, _options);

            // Act
            var result = await repo.GetWeather(-41.3, 174.7);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9.45, result.Main.Temp); //ensure temp set
            Assert.Equal(15.0, result.Wind.Speed); //ensure wind set
            Assert.Equal(800, result.Weather.FirstOrDefault().Id); //ensure id returns
        }
    }
}
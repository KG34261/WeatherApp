using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Test.Builders
{
    public static class HttpClientBuilder
    {
        public static HttpClient CreateClient(HttpStatusCode statusCode, string content, out Mock<HttpMessageHandler> handlerMock)
        {
            handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                });

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://fake-weather.com")
            };
        }
    }
}

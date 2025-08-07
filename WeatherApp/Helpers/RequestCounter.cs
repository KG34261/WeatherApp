using WeatherApp.Interface.Helpers;

namespace WeatherApp.Helpers
{


    public class RequestCounter : IRequestCounter
    {
        private static int _requestCount = 0;
        private static readonly object _lock = new();
        /// <summary>
        /// Mocks a downstreamfailure every 5 requests. Resets on service restart
        /// </summary>
        /// <returns></returns>
        public bool ShouldFail()
        {
            int currentCount;

            lock (_lock)
            {
                _requestCount++;
                currentCount = _requestCount;
            }

            return currentCount % 5 == 0;
        }
    }


}

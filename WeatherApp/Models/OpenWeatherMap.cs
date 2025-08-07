namespace WeatherApp.Models
{

        public class OpenWeatherMap
        {
            /// <summary>
            /// A list of weather conditions.
            /// </summary>
            public List<WeatherInfo> Weather { get; set; }

            /// <summary>
            /// Contains temperature-related information.
            /// </summary>
            public MainInfo Main { get; set; }

            /// <summary>
            /// Contains wind-related information.
            /// </summary>
            public WindInfo Wind { get; set; }
        }

        /// <summary>
        /// Represents a specific weather condition, including summary and icon.
        /// </summary>
        public class WeatherInfo
        {
            /// <summary>
            /// Weather condition ID.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Basic weather description
            /// </summary>
            public string Main { get; set; }

            /// <summary>
            /// Detailed weather description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Weather icon ID used to retrieve graphical icons.
            /// </summary>
            public string Icon { get; set; }
        }
        /// <summary>
        /// Contains main temperature data.
        /// </summary>
        public class MainInfo
        {
            /// <summary>
            /// Temperature in degrees
            /// </summary>
            public double Temp { get; set; }
        }
        /// <summary>
        /// Contains wind speed and direction information.
        /// </summary>
        public class WindInfo
        {
            /// <summary>
            /// Wind speed in km/h
            /// </summary>
            public double Speed { get; set; }
            /// <summary>
            /// Wind direction in degrees (meteorological).
            /// </summary>
            public int Deg { get; set; }

            /// <summary>
            /// Wind gust speed in km/h.
            /// </summary>
            public double Gust { get; set; }
        }

    }

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        /// <summary>
        /// Temperature in degrees
        /// </summary>
        public double Temperature { get; set; }
        /// <summary>
        /// Wind speed in km/h
        /// </summary>
        public double WindSpeed { get; set; }
        /// <summary>
        /// Basic weather description
        /// </summary>
        public string WeatherCondition { get; set; }
        /// <summary>
        /// List of recommendations for the day
        /// </summary>
        public List<string> Recommendations { get; set; }
        /// <summary>
        /// Current units of measurements
        /// </summary>
        public string MeasureUnits { get; set; } =  Units.Metric.ToString();
    }
    /// <summary>
    /// list of possible WeatherConditions
    /// </summary>
    public enum WeatherCondition
    {
        Sunny,
        Windy,
        Rainy,
        Snowing
    }
    /// <summary>
    /// Units of measurements
    /// </summary>
    public enum Units
    {
        Metric,
        Imperial
    }
}

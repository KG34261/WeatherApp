namespace WeatherApp.Helpers
{
    public static class LatLongHelper
    {
        /// <summary>
        /// Checkes for Valid Latitude 
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static bool IsValidLatitude(double latitude)
        {
            return latitude >= -90.0 && latitude <= 90.0;
        }
        /// <summary>
        /// Checks for valid Longitude
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static bool IsValidLongitude(double longitude)
        {
            return longitude >= -180.0 && longitude <= 180.0;
        }
        /// <summary>
        /// Provides both latitude and longitude check
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static bool IsValidCoordinates(double latitude, double longitude)
        {
            return IsValidLatitude(latitude) && IsValidLongitude(longitude);
        }

    }
}

using WeatherApp.Helpers;

namespace WeatherApp.Test
{
    public class LatLongHelperTest
    {
        [Theory]
        [InlineData(-90.0)]
        [InlineData(0.0)]
        [InlineData(90.0)]
        public void IsValidLatitude_ValidValues_True(double latitude)
        {
            Assert.True(LatLongHelper.IsValidLatitude(latitude));
        }
        [Theory]
        [InlineData(-90.1)]
        [InlineData(90.1)]
        [InlineData(1000.0)]
        public void isValidLatitude_Invalid_False(double latitude)
        {
            Assert.False(LatLongHelper.IsValidLatitude(latitude));
        }
        [Theory]
        [InlineData(-180.0)]
        [InlineData(0.0)]
        [InlineData(180.0)]
        public void IsValidLongitude_ValidValues_ReturnsTrue(double lon)
        {
            Assert.True(LatLongHelper.IsValidLongitude(lon));
        }
        [Theory]
        [InlineData(-180.1)]
        [InlineData(180.1)]
        [InlineData(999.0)]
        public void IsValidLongitude_InvalidValues_ReturnsFalse(double lon)
        {
            Assert.False(LatLongHelper.IsValidLongitude(lon));
        }
    }
}
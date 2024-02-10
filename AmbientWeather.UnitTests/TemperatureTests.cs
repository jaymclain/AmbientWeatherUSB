namespace AmbientWeather.UnitTests
{
    public class TemperatureTests
    {
        [Theory]
        [InlineData(0.0d, 0.0d)]
        [InlineData(-40.0d, -40.0d)]
        [InlineData(20.0d, 20.0d)]
        public void WhenInvoked_Celsius_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Celsius.ShouldBe(expected);
        }
        
        [Theory]
        [InlineData(0.0d, 32.0d)]
        [InlineData(-40.0d, -40.0d)]
        [InlineData(20.0d, 68.0d)]
        public void WhenInvoked_Farenheit_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Farenheit.ShouldBe(expected);
        }
        
        [Theory]
        [InlineData(0.0d, 273.15d)]
        [InlineData(-273.15d, 0.0d)]
        [InlineData(273.15d, 546.3d)]
        public void WhenInvoked_Kelvin_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Kelvin.ShouldBe(expected);
        }
    }
}

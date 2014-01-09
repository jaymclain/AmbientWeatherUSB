using AmbientWeather;
using NUnit.Framework;
using Should;

namespace UnitTests
{
    [TestFixture]
    public class TemperatureTests
    {
        [Test]
        [TestCase(0.0d, 0.0d)]
        [TestCase(-40.0d, -40.0d)]
        [TestCase(20.0d, 20.0d)]
        public void WhenInvoked_Celsius_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Celsius.ShouldEqual(expected);
        }
        
        [Test]
        [TestCase(0.0d, 32.0d)]
        [TestCase(-40.0d, -40.0d)]
        [TestCase(20.0d, 68.0d)]
        public void WhenInvoked_Farenheit_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Farenheit.ShouldEqual(expected);
        }
        
        [Test]
        [TestCase(0.0d, 273.15d)]
        [TestCase(-273.15d, 0.0d)]
        [TestCase(273.15d, 546.3d)]
        public void WhenInvoked_Kelvin_ShouldConvert(double input, double expected)
        {
            new Temperature(input).Kelvin.ShouldEqual(expected);
        }
    }
}

using HidLibrary;

namespace AmbientWeather;

public class WeatherStations : List<IWeatherStation>, IWeatherStations
{
    private const int AmbientWeatherVendorId = 0x1941;
    private const int AmbientWeatherProductId = 0x8021;

    public WeatherStations()
    {
        var devices = HidDevices.Enumerate(AmbientWeatherVendorId, AmbientWeatherProductId);

        foreach (var device in devices)
            Add(new WeatherStation(device));
    }
}
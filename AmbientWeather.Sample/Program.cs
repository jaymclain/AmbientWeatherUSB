using System.Text.Json;

namespace AmbientWeather.Sample;

internal class Program
{
    private static void Main()
    {
        var weatherStations = new WeatherStations();
        var weatherStation = weatherStations.First();

        weatherStation.SettingsLoaded += WeatherStationSettingsLoaded;
        weatherStation.HistoryData += WeatherStationHistoryData;
            
        Console.WriteLine("Press any key to quit.");
        Console.ReadKey();
    }

    private static void WeatherStationHistoryData(IWeatherStation weatherStation, HistoryData historyDataReport)
    {
        Console.WriteLine(JsonSerializer.Serialize(historyDataReport));
    }

    static void WeatherStationSettingsLoaded(IWeatherStation weatherStation, Settings stationReport)
    {
        Console.WriteLine(JsonSerializer.Serialize(stationReport));
    }
}
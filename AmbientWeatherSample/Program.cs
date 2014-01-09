using AmbientWeather;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AmbientWeatherSample
{
    class Program
    {
        static void Main()
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
            Console.WriteLine(JsonConvert.SerializeObject(historyDataReport));
        }

        static void WeatherStationSettingsLoaded(IWeatherStation weatherStation, Settings stationReport)
        {
            Console.WriteLine(JsonConvert.SerializeObject(stationReport));
        }
    }
}

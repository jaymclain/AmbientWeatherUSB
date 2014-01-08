using AmbientWeather;
using System;
using Newtonsoft.Json;

namespace AmbientWeatherSample
{
    class Program
    {
        static void Main()
        {
            var weatherStation = WeatherStation.OpenDevice();

            //weatherStation.SettingsReported += weatherStation_SettingsReported;
            //weatherStation.DownloadSettings();

            weatherStation.HistoryDataReported += weatherStation_HistoryDataReported;
            weatherStation.DownloadHistoryData();
            
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        private static void weatherStation_HistoryDataReported(HistoryDataReport historyDataReport)
        {
            Console.WriteLine(JsonConvert.SerializeObject(historyDataReport));
        }

        static void weatherStation_SettingsReported(SettingsReport stationReport)
        {
            Console.WriteLine(JsonConvert.SerializeObject(stationReport));
        }
    }
}

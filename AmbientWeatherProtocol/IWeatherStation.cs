using System.Collections;
using System.Collections.Generic;

namespace AmbientWeather
{
    public interface IWeatherStation
    {
        bool Connected { get; }

        Settings Settings { get; }
        IEnumerable<HistoryData> History { get; }

        event WeatherStationConnectedEventHandler WeatherStationConnected;
        event WeatherStationDisconnectedEventHandler WeatherStationDisconnected;
        event SettingsLoadedEventHandler SettingsLoaded;
        event HistoryDataEventHandler HistoryData;
    }
}

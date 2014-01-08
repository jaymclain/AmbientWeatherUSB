namespace AmbientWeather
{
    public interface IWeatherStation
    {
        void DownloadSettings();
        void DownloadHistoryData();

        bool Connected { get; }

        event WeatherStationConnectedEventHandler WeatherStationConnected;
        event WeatherStationDisconnectedEventHandler WeatherStationDisconnected;
        event SettingsReportEventHandler SettingsReported;
        event HistoryDataReportEventHandler HistoryDataReported;
    }
}

using HidLibrary;

namespace AmbientWeather;

public delegate void HistoryDataEventHandler(IWeatherStation weatherStation, HistoryData historyDataReport);
public delegate void SettingsLoadedEventHandler(IWeatherStation weatherStation, Settings settingsReport);
public delegate void WeatherStationConnectedEventHandler(IWeatherStation weatherStation);
public delegate void WeatherStationDisconnectedEventHandler(IWeatherStation weatherStation);

public class WeatherStation : IWeatherStation, IDisposable
{
    private const byte WriteCommand = 0xA0;
    private const byte ReadCommand = 0xA1;
    private const byte WriteCommandWord = 0xA2;
    private const byte EndMark = 0x20;

    private const int DataReadLength = 0x20;        // 32 bytes
    private const int DataReadBlockLength = 0x08;   // 8 bytes

    private const int SettingsStartAddress = 0x0000;
    private const int SettingsEndAddress = 0x00FF;

    private const int HistoryDataStartAddress = 0x0100;
    private const int HistoryDataEndAddress = 0xFFFF;
    private const int HistoryDataBlockLength = 0x10;

    private readonly HidDevice _weatherStation;
    private readonly List<HistoryData> _history = []; 

    public WeatherStation(HidDevice hidDevice)
    {
        _weatherStation = hidDevice;

        _weatherStation.Inserted += _weatherStation_Inserted;
        _weatherStation.Removed += _weatherStation_Removed;

        if (!_weatherStation.IsOpen)
            _weatherStation.OpenDevice();

        _weatherStation.MonitorDeviceEvents = true;
    }

    ~WeatherStation()
    {
        Dispose();
    }

    public Settings? Settings { get; private set; }

    public event SettingsLoadedEventHandler? SettingsLoaded;

    protected virtual void OnSettingsLoaded(IWeatherStation weatherStation, Settings settings)
    {
        var handler = SettingsLoaded;
        handler?.Invoke(weatherStation, settings);
    }

    public IEnumerable<HistoryData> History => _history;

    public event HistoryDataEventHandler? HistoryData;

    protected virtual void OnHistoryData(IWeatherStation weatherStation, HistoryData historyDataReport)
    {
        var handler = HistoryData;
        handler?.Invoke(weatherStation, historyDataReport);
    }

    public event WeatherStationConnectedEventHandler? WeatherStationConnected;

    protected virtual void OnWeatherStationConnected(IWeatherStation weatherStation)
    {
        var handler = WeatherStationConnected;
        handler?.Invoke(weatherStation);
    }

    public event WeatherStationDisconnectedEventHandler? WeatherStationDisconnected;

    protected virtual void OnWeatherStationDisconnected(IWeatherStation weatherStation)
    {
        var handler = WeatherStationDisconnected;
        handler?.Invoke(weatherStation);
    }

    public bool Connected { get; private set; }

    private void _weatherStation_Inserted()
    {
        Connected = true;
        OnWeatherStationConnected(this);

        DownloadSettings();
        DownloadHistoryData();
    }

    private void _weatherStation_Removed()
    {
        Connected = false;
        OnWeatherStationDisconnected(this);
    }

    private void DownloadSettings()
    {
        var report = new Report(SettingsEndAddress - SettingsStartAddress);

        // Download settings from 0000h-0100h
        for (var a = SettingsStartAddress; a <= SettingsEndAddress; a += DataReadLength)
        {
            DownloadBlock(a, d =>
            {
                report.AddRawData(d);
                return true;
            });
        }

        // Create data object from report
        Settings = report.GetObject<Settings>();
        if (Settings != null)
            OnSettingsLoaded(this, Settings);
    }

    private void DownloadHistoryData()
    {
        for (var a = HistoryDataStartAddress; a < HistoryDataEndAddress; a += DataReadLength)
        {
            if (!DownloadBlock(a, ProcessHistoryDataBlock))
                break;
        }
    }

    private bool ProcessHistoryDataBlock(IEnumerable<byte> dataBytes)
    {
        var endOfStreamFound = false;
        var data = dataBytes.ToArray();

        for (var b = 0; b < DataReadLength; b += HistoryDataBlockLength)
        {
            var dataBlock = data.Skip(b).Take(HistoryDataBlockLength).ToArray();

            // If we find an "empty" data row, we are done.
            if (dataBlock.All(e => e == 0xFF))
            {
                endOfStreamFound = true;
                break;
            }

            var report = new Report(HistoryDataBlockLength);
            report.AddRawData(dataBlock);

            var historyData = report.GetObject<HistoryData>();
            if (historyData == null)
                continue;
            
            _history.Add(historyData);
            OnHistoryData(this, historyData);
        }

        return !endOfStreamFound;
    }

    private bool DownloadBlock(int address, Func<IEnumerable<byte>, bool> onDataAction)
    {
        var data = new byte[9];
        data[0] = 0x00;
        data[1] = ReadCommand;
        data[2] = (byte)(address / 0x0100);
        data[3] = (byte)(address % 0x0100);
        data[4] = EndMark;
        data[5] = ReadCommand;
        data[6] = 0x00;
        data[7] = 0x00;
        data[8] = EndMark;

        _weatherStation.Write(data);

        // Read 32 bytes total in blocks of 8 bytes
        var readData = new List<byte>();
        for (var i = 0; i < DataReadLength; i += DataReadBlockLength)
        {
            var blockData = _weatherStation.Read();
            readData.AddRange(blockData.Data.Skip(1));
        }

        return onDataAction(readData);
    }

    public void Dispose()
    {
        _weatherStation.CloseDevice();
        GC.SuppressFinalize(this);
    }
}
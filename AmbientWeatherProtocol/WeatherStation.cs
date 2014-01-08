using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmbientWeather
{
    public class WeatherStation : IDisposable
    {
        private const int AmbientWeatherVendorId = 0x1941;
        private const int AmbientWeatherProductId = 0x8021;

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

        public static WeatherStation OpenDevice()
        {
            return HidDevices.Enumerate(AmbientWeatherVendorId, AmbientWeatherProductId).Select(x => new WeatherStation(x)).FirstOrDefault();
        }

        public delegate void SettingsReportEventHandler(SettingsReport settingsReport);
        public event SettingsReportEventHandler SettingsReported;

        protected virtual void OnSettingsReport(SettingsReport settingsReport)
        {
            var handler = SettingsReported;
            if (handler != null) handler(settingsReport);
        }

        public delegate void HistoryDataReportEventHandler(HistoryDataReport historyDataReport);
        public event HistoryDataReportEventHandler HistoryDataReported;

        protected virtual void OnHistoryDataReport(HistoryDataReport historyDataReport)
        {
            var handler = HistoryDataReported;
            if (handler != null) handler(historyDataReport);
        }

        public delegate void WeatherStationConnectedEventHandler();
        public event WeatherStationConnectedEventHandler WeatherStationConnected;

        protected virtual void OnWeatherStationConnected()
        {
            var handler = WeatherStationConnected;
            if (handler != null) handler();
        }

        public delegate void WeatherStationDisconnectedEventHandler();
        public event WeatherStationDisconnectedEventHandler WeatherStationDisconnected;

        protected virtual void OnWeatherStationDisconnected()
        {
            var handler = WeatherStationDisconnected;
            if (handler != null) handler();
        }

        public bool Connected { get; private set; }

        private void _weatherStation_Inserted()
        {
            Connected = true;
            OnWeatherStationConnected();
        }

        private void _weatherStation_Removed()
        {
            Connected = false;
            OnWeatherStationDisconnected();
        }

        public void DownloadSettings()
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
            var stationReport = report.GetObject<SettingsReport>();
            OnSettingsReport(stationReport);
        }

        public void DownloadHistoryData()
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

                var historyDataReport = report.GetObject<HistoryDataReport>();
                OnHistoryDataReport(historyDataReport);
            }

            return !endOfStreamFound;
        }

        public bool DownloadBlock(int address, Func<IEnumerable<byte>, bool> onDataAction)
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
}

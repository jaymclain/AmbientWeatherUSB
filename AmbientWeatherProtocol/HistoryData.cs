using System.Runtime.InteropServices;
using AmbientWeather.Extensions;

// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnassignedReadonlyField.Compiler

namespace AmbientWeather
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
    public class HistoryData
    {
        public readonly byte SampleInterval;
        public readonly byte IndoorPercentHumidity;
        private readonly ushort IndoorTemperatureData;
        public Temperature IndoorTemperature { get { return new Temperature(IndoorTemperatureData.MsbSigned() * 0.1d); } }
        public readonly byte OutdoorPercentHumidity;
        private readonly ushort OutdoorTemperatureData;
        public Temperature OutdoorTemperature { get { return new Temperature(OutdoorTemperatureData.MsbSigned() * 0.1d); } }
        private readonly ushort AbsolutePressureData;
        public double AbsolutePressure { get { return AbsolutePressureData * 0.1d; } }
        private readonly byte AverageWindSpeedData;
        public double AverageWindSpeed { get { return AverageWindSpeedData * 0.1d; } }
        private readonly byte GustWindSpeedData;
        public double GustWindSpeed { get { return GustWindSpeedData * 0.1d; } }
        private readonly byte WindSpeedHighData;
        public double GustWindSpeedHigh { get { return (WindSpeedHighData & 0xF0) >> 4; } }
        public double AverageWindSpeedHigh { get { return (WindSpeedHighData & 0x0F); } }
        private readonly byte WindDirectionData;
        public WindDirection WindDirection { get { return (WindDirection)WindDirectionData; } }
        private readonly ushort TotalRainfallData;
        public double TotalRainfall { get { return TotalRainfallData * 0.3d; } }
        private readonly byte StatusData;
        public bool TotalRainfallValid { get { return (((StatusData & 0x40) >> 6) == 0); } }
        public bool RainfallOverflow { get { return (((StatusData & 0x80) >> 7) == 1); } }
    }

    public enum WindDirection
    {
        North = 0,
        NorthNorthEast = 1,
        NorthEast = 2,
        EastNorthEast = 3,
        East = 4,
        EastSouthEast = 5,
        SouthEast = 6,
        SouthSouthEast = 7,
        South = 8,
        SouthSouthWest = 9,
        SouthWest = 10,
        WestSouthWest = 11,
        West = 12,
        WestNorthWest = 13,
        NorthWest = 14,
        NorthNorthWest = 15
    }
}

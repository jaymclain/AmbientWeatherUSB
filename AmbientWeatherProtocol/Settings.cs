using System;
using System.Linq;
using System.Runtime.InteropServices;
using AmbientWeather.Extensions;

#pragma warning disable 169
#pragma warning disable 649

// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedReadonlyField

namespace AmbientWeather
{
    [StructLayout(LayoutKind.Sequential, Pack=1, Size=256)]
    public class Settings
    {
        private readonly byte EepromInitializedFlag1;
        private readonly byte EepromInitailizedFlag2;

        public readonly ushort Model;
        public readonly byte Version;
        public readonly ushort ID;

        public readonly ushort PrecipitationSystemVersion;
        public readonly ushort WindSpeedSystemVersion;
        public readonly ushort NumberOfPrecipitationSystems;
        public readonly ushort NumberOfWindSpeedSystems;

        private readonly byte empty;

        private readonly byte SamplingIntervalData;
        public TimeSpan SamplingInterval { get { return TimeSpan.FromMinutes(SamplingIntervalData); } }

        private readonly byte CurrentUnitSettingFlag1;
        public TemperatureUnit IndoorTemperatureUnit { get { return (TemperatureUnit)(CurrentUnitSettingFlag1 & 0x01); } }
        public TemperatureUnit OutdoorTemperatureUnit { get { return (TemperatureUnit)((CurrentUnitSettingFlag1 & 0x02) >> 1); } }
        public RainfallUnits RainfallUnits { get { return (RainfallUnits)((CurrentUnitSettingFlag1 & 0x04) >> 2); } }
        public PressureUnits PressureUnits { get { return (PressureUnits)((CurrentUnitSettingFlag1 & 0xE0) >> 5); } }

        private readonly byte CurrentUnitSettingFlag2;
        public WindSpeedUnits WindSpeedUnits { get { return (WindSpeedUnits)(CurrentUnitSettingFlag2 & 0x1F); } }

        private readonly byte DisplayFormatFlag1;
        public PressureType PressureType { get { return (PressureType)(DisplayFormatFlag1 & 0x01); } }
        private readonly byte DisplayFormatFlag2;
        private readonly byte AlarmEnableFlag1;
        private readonly byte AlarmEnableFlag2;
        private readonly byte AlarmEnableFlag3;
        private readonly byte TimeZoneData;
        public TimeZoneInfo TimeZone { get { return TimeZoneInfo.GetSystemTimeZones().First(z => z.BaseUtcOffset.Hours == TimeZoneData.MsbSigned()); } }
        private readonly byte Reserved1;
        private readonly byte DataRefreshed;
        private readonly ushort HistoryDataSets;
        private readonly byte Reserved2;
        private readonly ushort HistoryDataStartAddress;
        private readonly ushort RelativePressure;
        private readonly ushort AbsolutePressure;
        private readonly ushort Reserved3;
        private readonly ushort WindCorrection;
        private readonly ushort OutdoorTemperatureOffsetData;
        public short OutdoorTemperatureOffset { get { return OutdoorTemperatureOffsetData.MsbSigned(); } }

        private readonly ushort IndoorTemperatureOffsetData;
        public short IndoorTemperatureOffset { get { return IndoorTemperatureOffsetData.MsbSigned(); } }

        private readonly ushort OutdoorHumidityOffsetData;
        public short OutdoorHumidityOffset { get { return OutdoorHumidityOffsetData.MsbSigned(); } }
        private readonly ushort IndoorHumidityOffsetData;
        public short IndoorHumidityOffset { get { return IndoorHumidityOffsetData.MsbSigned(); } }

        private readonly byte IndoorHumdityHighAlarm;
        private readonly byte IndoorHumdityLowAlarm;
        private readonly ushort IndoorTemperatureHighAlarm;
        private readonly ushort IndoorTemperatureLowAlarm;
        private readonly byte OutdoorHumdityHighAlarm;
        private readonly byte OutdoorHumdityLowAlarm;
        private readonly ushort OutdoorTemperatureHighAlarm;
        private readonly ushort OutdoorTemperatureLowAlarm;
        private readonly ushort WindChillHighAlarm;
        private readonly ushort WindChillLowAlarm;
        private readonly ushort DewPointHighAlarm;
        private readonly ushort DewPointLowAlarm;
        private readonly ushort AbsolutePressureHighAlarm;
        private readonly ushort AbsolutePressureLowAlarm;
        private readonly ushort RelativePressureHighAlarm;
        private readonly ushort RelativePressureLowAlarm;

    }

    public enum RainfallUnits
    {
        Millimeters = 0,
        Inches = 1
    }

    public enum PressureUnits
    {
        Hpa = 1,
        inHg = 2,
        mmHg = 3
    }

    public enum WindSpeedUnits
    {
        MetersPerSecond = 1,
        KilometersPerHour = 2,
        Knots = 4,
        MilesPerHour = 8,
        Beaufort = 16
    }

    public enum BeaufortScale
    {
        Calm = 0,
        LightAir = 1,
        LightBreeze = 2,
        GentleBreeze = 3,
        ModerateBreeze = 4,
        FreshBreeze = 5,
        StrongBreeze = 6,
        HighWind = 7,
        ModerateGale = 7,
        NearGale = 7,
        Gale = 8,
        FreshGale = 8,
        StrongGale = 9,
        Storm = 10,
        WholeGale = 10,
        ViolentStorm = 11,
        HurricaneForce = 12
    }

    public enum PressureType
    {
        Absolute = 0,
        Relative = 1
    }

    public enum WindSpeedType
    {
        Average = 0,
        Gust = 1,
    }

    public enum TimeType
    {
        TwentyFourHour,
        TwelveHour,
    }
}

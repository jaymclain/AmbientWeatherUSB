using System.Runtime.InteropServices;

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

        public readonly byte SamplingInterval;   // 1-240 Minutes

        private readonly byte CurrentUnitSettingFlag1;
        public TemperatureUnits IndoorTemperatureUnits { get { return (TemperatureUnits)(CurrentUnitSettingFlag1 & 0x01); } }
        public TemperatureUnits OutdoorTemperatureUnits { get { return (TemperatureUnits)((CurrentUnitSettingFlag1 & 0x02) >> 1); } }
        public RainfallUnits RainfallUnits { get { return (RainfallUnits)((CurrentUnitSettingFlag1 & 0x04) >> 2); } }
        public PressureUnits PressureUnits { get { return (PressureUnits)((CurrentUnitSettingFlag1 & 0xE0) >> 5); } }

        private readonly byte CurrentUnitSettingFlag2;
        public WindSpeedUnits WindSpeedUnits { get { return (WindSpeedUnits)(CurrentUnitSettingFlag2 & 0x1F); } }

        private readonly byte DisplayFormatFlag1;
        public PressureType PressureType { get { return (PressureType)(DisplayFormatFlag1 & 0x01); } }
        private readonly byte DisplayFormatFlag2;
    }

    public enum TemperatureUnits
    {
        Celsius = 0,
        Fahrenheit = 1,
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

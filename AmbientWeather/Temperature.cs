using System.Globalization;

namespace AmbientWeather;

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin,
    Rankine,
    Delisle,
    Newton,
    Raeumur,
    Romer
}

public class Temperature
{
    private readonly double _celsiusTemperature;

    private readonly Dictionary<TemperatureUnit, Func<double, double>> _converters =
        new()
        {
            { TemperatureUnit.Celsius,      t => t },
            { TemperatureUnit.Fahrenheit,   t => (t * 1.8d) + 32.0d },
            { TemperatureUnit.Kelvin,       t => t + 273.15d },
            { TemperatureUnit.Rankine,      t => (t + 273.15d) + 1.8d },
            { TemperatureUnit.Delisle,      t => (100.0d - t) * 1.5d },
            { TemperatureUnit.Newton,       t => t * 0.33d },
            { TemperatureUnit.Raeumur,      t => t * 0.8d },
            { TemperatureUnit.Romer,        t => (t * 0.525d) + 7.5d }
        };

    public Temperature(double celsiusTemperature)
    {
        _celsiusTemperature = celsiusTemperature;
    }

    public double Celsius => _converters[TemperatureUnit.Celsius].Invoke(_celsiusTemperature);

    public double Fahrenheit => _converters[TemperatureUnit.Fahrenheit].Invoke(_celsiusTemperature);

    public double Kelvin => _converters[TemperatureUnit.Kelvin].Invoke(_celsiusTemperature);

    public double Rankine => _converters[TemperatureUnit.Rankine].Invoke(_celsiusTemperature);

    public double Delisle => _converters[TemperatureUnit.Delisle].Invoke(_celsiusTemperature);

    public double Newton => _converters[TemperatureUnit.Newton].Invoke(_celsiusTemperature);

    public double Raeumur => _converters[TemperatureUnit.Raeumur].Invoke(_celsiusTemperature);

    public double Romer => _converters[TemperatureUnit.Romer].Invoke(_celsiusTemperature);

    public override string ToString()
    {
        return ToString(null);
    }

    public string ToString(IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;

        return string.Format(formatProvider, "{0:F2}  °C", _celsiusTemperature);
    }
}
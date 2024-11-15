﻿using System.Globalization;

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

public class Temperature(double celsiusTemperature)
{
    private static readonly Dictionary<TemperatureUnit, Func<double, double>> _converters =
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

    public double Celsius => _converters[TemperatureUnit.Celsius].Invoke(celsiusTemperature);

    public double Fahrenheit => _converters[TemperatureUnit.Fahrenheit].Invoke(celsiusTemperature);

    public double Kelvin => _converters[TemperatureUnit.Kelvin].Invoke(celsiusTemperature);

    public double Rankine => _converters[TemperatureUnit.Rankine].Invoke(celsiusTemperature);

    public double Delisle => _converters[TemperatureUnit.Delisle].Invoke(celsiusTemperature);

    public double Newton => _converters[TemperatureUnit.Newton].Invoke(celsiusTemperature);

    public double Raeumur => _converters[TemperatureUnit.Raeumur].Invoke(celsiusTemperature);

    public double Romer => _converters[TemperatureUnit.Romer].Invoke(celsiusTemperature);

    public override string ToString()
    {
        return ToString(null);
    }

    public string ToString(IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;

        return string.Format(formatProvider, "{0:F2}  °C", celsiusTemperature);
    }
}
using System;
using System.Globalization;

namespace Services.Utilities;

public static class NumberAbbreviator
{
    private static string Abbreviate(decimal number)
    {
        var result = Math.Abs(number) switch
        {
            // Hundred Million
            // >= 1.0E8m => (number / 1.0E8m).ToString("F1", culture),
            // Million
            >= 1000000m => decimal.Floor(number / 1000000m) + "M",
            // Ten Thousand
            // >= 1.0E4m => (number / 1.0E4m).ToString("F1", culture),
            // Thousand
            >= 1000m => decimal.Floor(number / 1000m) + "K",
            _        => number.ToString(CultureInfo.InvariantCulture)
        };

        return result;
    }
}
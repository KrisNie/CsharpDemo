using System;
using System.Collections.Generic;
using Collections.DataStructure.List;

namespace Collections.DataStructure.Common;

public static class Helper
{
    /// <summary>
    /// Swaps two values in <![CDATA[an IList<T>]]> collection given their indexes.
    /// </summary>
    public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
    {
        if (list.Count < 2 || firstIndex == secondIndex) return;
        (list[firstIndex], list[secondIndex]) = (list[secondIndex], list[firstIndex]);
    }

    /// <summary>
    /// Swaps two values in <![CDATA[an ArrayList<T>]]> collection given their indexes.
    /// </summary>
    public static void Swap<T>(this ArrayList<T> list, int firstIndex, int secondIndex)
    {
        if (list.Count < 2 || firstIndex == secondIndex) return;
        (list[firstIndex], list[secondIndex]) = (list[secondIndex], list[firstIndex]);
    }

    /// <summary>
    /// Centralize a text.
    /// </summary>
    public static string PadCenter(this string text, int newWidth, char fillerCharacter = ' ')
    {
        if (string.IsNullOrEmpty(text)) return text;
        var length = text.Length;
        var charactersToPad = newWidth - length;
        if (charactersToPad < 0)
            throw new ArgumentException(
                "New width must be greater than string length.",
                nameof(newWidth));
        var padLeft = charactersToPad / 2 + charactersToPad % 2;
        // Add a space to the left if the string is an odd number
        var padRight = charactersToPad / 2;

        return new string(fillerCharacter, padLeft) + text + new string(fillerCharacter, padRight);
    }

    /// <summary>
    /// Populates the specified two-dimensional with a default value.
    /// </summary>
    public static void Populate<T>(this T[,] array, int rows, int columns, T defaultValue = default)
    {
        for (var i = 0; i < rows; ++i)
        {
            for (var j = 0; j < columns; ++j) array[i, j] = defaultValue;
        }
    }
}
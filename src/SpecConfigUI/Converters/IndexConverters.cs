using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SpecConfigUI.Converters;

public class IndexGreaterThanZeroConverter : IValueConverter
{
    public static readonly IndexGreaterThanZeroConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index)
            return index > 0;
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class IndexLessThanCountMinusOneConverter : IValueConverter
{
    public static readonly IndexLessThanCountMinusOneConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // value - это индекс элемента
        // parameter - это коллекция, для которой нужно проверить Count
        if (value is int index && parameter is ICollection collection)
            return index < collection.Count - 1;
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

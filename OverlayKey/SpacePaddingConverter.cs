using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OverlayKey
{
    public class SpacePaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string s && s == "Space"
                ? new Thickness(100, 7, 100, 7)
                : new Thickness(12, 7, 12, 7);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

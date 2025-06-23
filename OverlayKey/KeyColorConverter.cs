using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OverlayKey
{
    public class KeyColorConverter : IValueConverter
    {
        public static Color NormalColor = Color.FromArgb(0, 255, 140, 0);
        public static Color PressedColor = Color.FromArgb(210, 100, 70, 220);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPressed = (bool)value;
            return new SolidColorBrush(isPressed ? PressedColor : NormalColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Windows.Media;

namespace OverlayKey
{
    public class UserSettings
    {
        public string KeyColor { get; set; }
        public string PressedColor { get; set; }

        public static UserSettings Default => new UserSettings
        {
            KeyColor = "#78FF8C00",
            PressedColor = "#D26446DC"
        };

        public Color KeyColorAsColor() => (Color)ColorConverter.ConvertFromString(KeyColor);
        public Color PressedColorAsColor() => (Color)ColorConverter.ConvertFromString(PressedColor);
    }
}

using System;
using System.Windows;
using System.Windows.Media;

namespace OverlayKey
{
    public partial class SettingsWindow : Window
    {
        public Action<Color, Color> OnApplyColors;

        public SettingsWindow(Color currentBg, Color currentPress)
        {
            InitializeComponent();

            BgColorPicker.SelectedColor = currentBg;
            PressColorPicker.SelectedColor = currentPress;
            BgColorBox.Text = $"#{currentBg.A:X2}{currentBg.R:X2}{currentBg.G:X2}{currentBg.B:X2}";
            PressColorBox.Text = $"#{currentPress.A:X2}{currentPress.R:X2}{currentPress.G:X2}{currentPress.B:X2}";
        }

        private void BgColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                var c = e.NewValue.Value;
                BgColorBox.Text = $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
            }
        }

        private void PressColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                var c = e.NewValue.Value;
                PressColorBox.Text = $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bg = (Color)ColorConverter.ConvertFromString(BgColorBox.Text);
                var press = (Color)ColorConverter.ConvertFromString(PressColorBox.Text);
                OnApplyColors?.Invoke(bg, press);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка цвета! Используй формат #AARRGGBB", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

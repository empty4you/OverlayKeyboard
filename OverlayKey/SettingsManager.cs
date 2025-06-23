using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace OverlayKey
{
    public static class SettingsManager
    {
        private const string SettingsFileName = "settings.json";

        public static UserSettings Settings = UserSettings.Default;

        public static void SaveSettings()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                File.WriteAllText(SettingsFileName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}");
            }
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFileName))
                {
                    var json = File.ReadAllText(SettingsFileName);
                    Settings = JsonSerializer.Deserialize<UserSettings>(json);
                }
                else
                {
                    Settings = UserSettings.Default;
                }
                KeyColorConverter.NormalColor = Settings.KeyColorAsColor();
                KeyColorConverter.PressedColor = Settings.PressedColorAsColor();
            }
            catch (Exception ex)
            {
                Settings = UserSettings.Default;
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}");
            }
        }
    }
}

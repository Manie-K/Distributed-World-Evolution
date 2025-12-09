using System.IO;
using System.Text.Json;

namespace Client.Common
{
    public class UserSettings
    {
        private JsonSerializerOptions options;

        public string PlayerName { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public float GlobalMusicVolume { get; set; }
        public float GlobalEffectVolume { get; set; }

        public UserSettings()
        {
            options = new JsonSerializerOptions { WriteIndented = true };
        }

        public void LoadUserSettings()
        {
            string folderPath = @"Settings";
            string fileName = "UserSettings.json";
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                UserSettings loadedSettings = JsonSerializer.Deserialize<UserSettings>(json);

                PlayerName = loadedSettings.PlayerName;
                ScreenWidth = loadedSettings.ScreenWidth;
                ScreenHeight = loadedSettings.ScreenHeight;
                GlobalMusicVolume = loadedSettings.GlobalMusicVolume;
                GlobalEffectVolume = loadedSettings.GlobalEffectVolume;
            }
            else
            {
                PlayerName = "";
                ScreenWidth = 1280;
                ScreenHeight = 720;
                GlobalMusicVolume = 0.5f;
                GlobalEffectVolume = 0.5f;
            }
        }

        public void SaveUserSettings()
        {
            string folderPath = @"Settings";
            string fileName = "UserSettings.json";
            string filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }
    }
}

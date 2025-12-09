using Client.Common;
using Client.Panels.Windows;
using Client.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework.Content;
using System.Drawing;

namespace Client
{
    public class GameManager
    {
        public SceneManager SceneManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public ContentManager ContentManager {  get; private set; }
        public Camera2D Camera { get; private set; }
        public UserSettings UserSettings { get; private set; }
        public InputManager InputManager { get; private set; }
        public WindowManager WindowManager { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public IConfigurationRoot AppConfig { get; private set; }
        public bool IsInGame { get; set; }

        public GameManager(ContentManager content)
        {
            AppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            UserSettings = new UserSettings();
            UserSettings.LoadUserSettings();
            ContentManager = content;
            ContentManager.RootDirectory = "Content";
            SceneManager = new();
            AudioManager = new AudioManager(ContentManager);
            InputManager = new InputManager();
            ClientManager = new ClientManager(AppConfig);
            AudioManager.SetGlobalMusicVolume(UserSettings.GlobalMusicVolume);
            AudioManager.SetGlobalEffectVolume(UserSettings.GlobalEffectVolume);
            Camera = new Camera2D(new Size(UserSettings.ScreenWidth, UserSettings.ScreenHeight));
            Camera.ResetPosition();
            IsInGame = false;
        }

        public void SetWindowManager()
        {
            WindowManager = new WindowManager(this);
            ClientManager.StartClient();
        }
    }
}

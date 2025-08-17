using Client.Common;
using Client.Rendering;
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
        public bool IsInGame { get; set; }

        public GameManager(ContentManager content)
        {
            UserSettings = new UserSettings();
            UserSettings.LoadUserSettings();
            ContentManager = content;
            ContentManager.RootDirectory = "Content";
            SceneManager = new();
            AudioManager = new AudioManager(ContentManager);
            AudioManager.SetGlobalMusicVolume(UserSettings.GlobalMusicVolume);
            AudioManager.SetGlobalEffectVolume(UserSettings.GlobalEffectVolume);
            Camera = new Camera2D(new Size(UserSettings.ScreenWidth, UserSettings.ScreenHeight));
            Camera.ResetPosition();
            IsInGame = false;
        }
    }
}

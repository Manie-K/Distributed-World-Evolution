using Client.Common;
using Client.Panels.Windows;
using Client.Rendering;
using Microsoft.Xna.Framework.Content;
using SharedLibrary;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;

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
        public TcpClient Client { get; private set; }
        public bool IsInGame { get; set; }

        private string serverIp;
        private int port;
        private Thread receiveThread;

        public GameManager(ContentManager content)
        {
            UserSettings = new UserSettings();
            UserSettings.LoadUserSettings();
            ContentManager = content;
            ContentManager.RootDirectory = "Content";
            SceneManager = new();
            AudioManager = new AudioManager(ContentManager);
            InputManager = new InputManager();
            AudioManager.SetGlobalMusicVolume(UserSettings.GlobalMusicVolume);
            AudioManager.SetGlobalEffectVolume(UserSettings.GlobalEffectVolume);
            Camera = new Camera2D(new Size(UserSettings.ScreenWidth, UserSettings.ScreenHeight));
            Camera.ResetPosition();
            IsInGame = false;

            serverIp = "127.0.0.1";
            port = 5000;
            StartClient();
        }

        public void SetWindowManager()
        {
            WindowManager = new WindowManager(this);
        }

        private void StartClient()
        {
            try
            {
                Client = new TcpClient(serverIp, port);

                receiveThread = new Thread(() =>
                {
                    MessageBase message = MessageManager.ReceiveMessage(Client);
                    InfoMessage stringMessage = (InfoMessage)message;
                    Console.WriteLine($"[Client] Received message: {stringMessage.MessageContent}");
                });
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Client] error: {e.Message}");
            }
        }

        public void CloseClient()
        {
            Client?.Close();
            receiveThread?.Join();
        }
    }
}

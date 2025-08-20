using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class MainMenuScene : IScene
    {
        private GameManager manager;
        private Game game;

        private Button playButton;
        private Button settingsButton;
        private Button quitButton;
        private Texture2D backGround;
        private Texture2D mainMenuPanel;
        private Texture2D userNamePanel;
        private TextBox textBox;
        private Button saveButton;
        private bool playButtonClicked;

        public MainMenuScene(GameManager manager, Game game)
        {
            this.manager = manager;
            this.game = game;

            mainMenuPanel = manager.ContentManager.Load<Texture2D>("UI/Scenes/MainMenuPanel");
            playButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/PlayButton"), null, null, new Vector2(418, 208), 447, 100 , Color.Gold);
            settingsButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SettingsButton"), null, null, new Vector2(406, 335), 467, 95, Color.Gold);
            quitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/ExitButton"), null, null, new Vector2(420, 460), 445, 95, Color.Gold);
            backGround = manager.ContentManager.Load<Texture2D>("UI/BG_Forest");
            textBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), new Vector2(445, 315), 385, 87, Color.Black);
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SubmitButton"), null, null, new Vector2(493, 446), 299, 99, Color.Gold);
            userNamePanel = manager.ContentManager.Load<Texture2D>("UI/Scenes/UsernamePanel");
            playButtonClicked = false;
        }

        public void Load()
        {
            manager.AudioManager.LoadSong("MainMenuSong", "Audio/Light Ambience 1");
            manager.AudioManager.PlaySong("MainMenuSong");
        }


        public void Update(GameTime gameTime)
        {
            manager.InputManager.Update();

            if (manager.InputManager.CheckIfLeftClick())
            {
                if (!playButtonClicked)
                {
                    if (playButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                    {
                        playButtonClicked = true;
                        if (!manager.UserSettings.PlayerName.Equals(""))
                        {
                            playButtonClicked = false;
                            manager.SceneManager.AddScene(new LobbyScene(manager));
                        }
                    }
                    if (settingsButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                    {
                        manager.SceneManager.AddScene(new SettingsScene(manager));
                    }
                    if (quitButton.CheckLeftClick(manager.InputManager.GetMousePosition())) game.Exit();
                }
                else
                {
                    textBox.CheckLeftClick(manager.InputManager.GetMousePosition());
                    if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()) && !textBox.CheckTextIfEmpty())
                    {
                        manager.UserSettings.PlayerName = textBox.GetText();
                        manager.UserSettings.SaveUserSettings();
                        playButtonClicked = false;
                        manager.SceneManager.AddScene(new LobbyScene(manager));
                    }
                }
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                playButtonClicked = false;
            }

            if (playButtonClicked)
            {
                saveButton.Update(manager.InputManager.GetMousePosition());
                textBox.Update();
            }
            else
            {
                playButton.Update(manager.InputManager.GetMousePosition());
                settingsButton.Update(manager.InputManager.GetMousePosition());
                quitButton.Update(manager.InputManager.GetMousePosition());
            }

            manager.InputManager.SetPreviousStates();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);

            if (!playButtonClicked)
            {
                spriteBatch.Draw(mainMenuPanel, new Rectangle(265, 90, 750, 540), Color.White);
                playButton.Draw(spriteBatch);
                settingsButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(userNamePanel, new Rectangle(330, 50, 612, 612), Color.White);
                textBox.Draw(spriteBatch);
                saveButton.Draw(spriteBatch);
            }
        }

    }
}

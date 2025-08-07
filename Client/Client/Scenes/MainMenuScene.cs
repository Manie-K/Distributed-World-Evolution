using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Client.Rendering;

namespace Client
{
    public class MainMenuScene : IScene
    {
        private ContentManager contentManager;
        private Button playButton;
        private Button settingsButton;
        private Button quitButton;
        private Texture2D backGround;
        private Texture2D mainMenuPanel;
        private SceneManager sceneManager;
        private MouseState previousMouseState;
        private Game game;
        private AudioManager audioManager;

        private Texture2D userNamePanel;
        private TextBox textBox;
        private Button saveButton;
        private bool playButtonClicked;

        public MainMenuScene(ContentManager contentManager, SceneManager sceneManager, Game game, AudioManager audiomanager)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.game = game;
            mainMenuPanel= contentManager.Load<Texture2D>("UI/Scenes/MainMenuPanel");
            playButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/PlayButton"), null, null, new Vector2(418, 208), 447, 100 , Color.Gold);
            settingsButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/SettingsButton"), null, null, new Vector2(406, 335), 467, 95, Color.Gold);
            quitButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/ExitButton"), null, null, new Vector2(420, 460), 445, 95, Color.Gold);
            backGround = contentManager.Load<Texture2D>("UI/BG_Forest");
            textBox = new TextBox(null, contentManager.Load<SpriteFont>("Fonts/ButtonFont"), new Vector2(445, 315), 385, 87, Color.Black);
            saveButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/SubmitButton"), null, null, new Vector2(493, 446), 299, 99, Color.Gold);
            userNamePanel = contentManager.Load<Texture2D>("UI/Scenes/UsernamePanel");
            this.audioManager = audiomanager;
            playButtonClicked = false;
        }

        public void Load()
        {
            audioManager.LoadSong("MainMenuSong", "Audio/Light Ambience 1");
            audioManager.PlaySong("MainMenuSong");
        }


        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (!playButtonClicked)
                {
                    if (playButton.CheckLeftClick(position))
                    {
                        playButtonClicked = true;
                    }
                    if (settingsButton.CheckLeftClick(position))
                    {
                        sceneManager.AddScene(new SettingsScene(contentManager, sceneManager, audioManager));
                        audioManager.MuteAll();
                    }
                    if (quitButton.CheckLeftClick(position)) game.Exit();
                }
                else
                {
                    textBox.CheckLeftClick(position);
                    if (saveButton.CheckLeftClick(position) && !textBox.CheckTextIfEmpty())
                    {
                        sceneManager.AddScene(new LobbyScene(contentManager, sceneManager, audioManager, textBox.GetText()));
                        audioManager.MuteAll();
                    }
                }
            }

            if (playButtonClicked)
            {
                saveButton.Update(position);
                textBox.Update();
            }
            else
            {
                playButton.Update(position);
                settingsButton.Update(position);
                quitButton.Update(position);
            }

            previousMouseState = currentMouseState;
        }


        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, camera.ScreenSize.Width, camera.ScreenSize.Height), Color.White);

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

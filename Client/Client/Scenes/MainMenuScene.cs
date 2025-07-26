using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Client
{

    public class MainMenuScene : IScene
    {
        private ContentManager contentManager;
        private Button PlayButton;
        private Button SettingsButton;
        private Button QuitButton;
        private Texture2D BackGround;
        private Texture2D MainMenuPanel;
        private SceneManager sceneManager;
        private MouseState previousMouseState;
        private Game game;
        private AudioManager audioManager;

        private Texture2D UserNamePanel;
        private TextBox TextBox;
        private Button SaveButton;
        private bool PlayButtonClicked;

        public MainMenuScene(ContentManager contentManager, SceneManager sceneManager, Game game, AudioManager audiomanager)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.game = game;
            this.MainMenuPanel= contentManager.Load<Texture2D>("UI/Scenes/MainMenuPanel");
            this.PlayButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/PlayButton"), null, null, new Vector2(418, 208), 447, 100 , Color.Gold);
            this.SettingsButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/SettingsButton"), null, null, new Vector2(406, 335), 467, 95, Color.Gold);
            this.QuitButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/ExitButton"), null, null, new Vector2(420, 460), 445, 95, Color.Gold);
            this.BackGround = contentManager.Load<Texture2D>("UI/BG_Forest");
            this.TextBox = new TextBox(null, contentManager.Load<SpriteFont>("Fonts/ButtonFont"), new Vector2(445, 315), 385, 87, Color.Black);
            this.SaveButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/SubmitButton"), null, null, new Vector2(493, 446), 299, 99, Color.Gold);
            this.UserNamePanel = contentManager.Load<Texture2D>("UI/Scenes/UsernamePanel");
            this.audioManager = audiomanager;
            this.PlayButtonClicked = false;
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
                if (!PlayButtonClicked)
                {
                    if (PlayButton.CheckLeftClick(position))
                    {
                        PlayButtonClicked = true;
                    }
                    if (SettingsButton.CheckLeftClick(position))
                    {
                        sceneManager.AddScene(new SettingsScene(contentManager, sceneManager, audioManager));
                        audioManager.MuteAll();
                    }
                    if (QuitButton.CheckLeftClick(position)) game.Exit();
                }
                else
                {
                    TextBox.CheckLeftClick(position);
                    if (SaveButton.CheckLeftClick(position) && !TextBox.CheckTextIfEmpty())
                    {
                        sceneManager.AddScene(new LobbyScene(contentManager, sceneManager, audioManager, TextBox.GetText()));
                        audioManager.MuteAll();
                    }
                }
            }

            if (PlayButtonClicked)
            {
                SaveButton.Update(position);
                TextBox.Update();
            }
            else
            {
                PlayButton.Update(position);
                SettingsButton.Update(position);
                QuitButton.Update(position);
            }

            previousMouseState = currentMouseState;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);

            if (!PlayButtonClicked)
            {
                spriteBatch.Draw(MainMenuPanel, new Rectangle(265, 90, 750, 540), Color.White);
                PlayButton.Draw(spriteBatch);
                SettingsButton.Draw(spriteBatch);
                QuitButton.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(UserNamePanel, new Rectangle(330, 50, 612, 612), Color.White);
                TextBox.Draw(spriteBatch);
                SaveButton.Draw(spriteBatch);
            }
        }

    }
}

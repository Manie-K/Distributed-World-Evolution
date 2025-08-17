using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Client
{ 
    public class LobbyScene : IScene
    {
        private GameManager manager;

        private Texture2D backGround;
        private SwitchPage switchPage;
        private Button createButton;
        private Button joinButton;
        private Button refreshButton;
        private Button exitButton;
        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        public LobbyScene(GameManager manager)
        {
            this.manager = manager;

            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Lobby_BG");
            switchPage = new SwitchPage(manager.ContentManager.Load<Texture2D>("UI/White Left"), manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                             manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(542, 628), manager.ContentManager);
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            joinButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/JoinButton"), null, null, new Vector2(953, 240), 180, 70, new Color(255, 255, 128));
            refreshButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/RefreshButton"), null, null, new Vector2(953, 330), 180, 70, new Color(255, 255, 128));
            createButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/CreateButton"), null, null, new Vector2(953, 420), 180, 70, new Color(255, 255, 128));

            previousKeyboardState = Keyboard.GetState();
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);
            bool isPressed = false;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (exitButton.CheckLeftClick(position))
                {
                    manager.SceneManager.RemoveScene();
                }

                switchPage.CheckLeftClick(position);
                if (createButton.CheckLeftClick(position))
                {
                    manager.SceneManager.AddScene(new CreateLobbyScene(manager, switchPage));
                }
                else if (joinButton.CheckLeftClick(position))
                {
                    manager.SceneManager.RemoveScene();
                    manager.SceneManager.AddScene(new GameScene(manager));
                }
                else if (refreshButton.CheckLeftClick(position))
                {
                    Debug.WriteLine("Refresh");
                }
                isPressed= true;    
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            exitButton.Update(position);
            createButton.Update(position);
            joinButton.Update(position);
            refreshButton.Update(position);
            switchPage.UpdateRows(position, isPressed);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPage.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            createButton.Draw(spriteBatch);
            joinButton.Draw(spriteBatch);
            refreshButton.Draw(spriteBatch);  
        }
    }
}

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
        private MouseState previousMouseState;

        public LobbyScene(GameManager manager)
        {
            this.manager = manager;

            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Lobby_BG");
            switchPage = new SwitchPage(manager.ContentManager.Load<Texture2D>("UI/White Left"), manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                             manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(542, 628), manager.ContentManager);
            joinButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/JoinButton"), null, null, new Vector2(953, 240), 180, 70, new Color(255, 255, 128));
            refreshButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/RefreshButton"), null, null, new Vector2(953, 330), 180, 70, new Color(255, 255, 128));
            createButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/CreateButton"), null, null, new Vector2(953, 420), 180, 70, new Color(255, 255, 128));
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);
            bool isPressed = false;
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
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

            createButton.Update(position);
            joinButton.Update(position);
            refreshButton.Update(position);
            switchPage.UpdateRows(position, isPressed);
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPage.Draw(spriteBatch);
            createButton.Draw(spriteBatch);
            joinButton.Draw(spriteBatch);
            refreshButton.Draw(spriteBatch);  
        }
    }
}

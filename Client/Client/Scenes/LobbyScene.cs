using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Client
{ 
    public class LobbyScene : IScene
    {
        private ContentManager contentManager;
        private Texture2D backGround;
        private SceneManager sceneManager;
        private SwitchPage switchPage;
        private Button createButton;
        private Button joinButton;
        private Button refreshButton;
        private MouseState previousMouseState;
        private AudioManager audioManager;


        private string playername;

        public LobbyScene(ContentManager ContentManager, SceneManager SceneManager, AudioManager Audiomanager, string PlayerName)
        {
            contentManager = ContentManager;
            sceneManager = SceneManager;
            backGround = contentManager.Load<Texture2D>("UI/Scenes/Lobby_BG");
            switchPage = new SwitchPage(contentManager.Load<Texture2D>("UI/White Left"), contentManager.Load<Texture2D>("UI/White Right"),
                                             contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(542, 628), contentManager);
            joinButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/JoinButton"), null, null, new Vector2(953, 240), 180, 70, new Color(255, 255, 128));
            refreshButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/RefreshButton"), null, null, new Vector2(953, 330), 180, 70, new Color(255, 255, 128));
            createButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/CreateButton"), null, null, new Vector2(953, 420), 180, 70, new Color(255, 255, 128));

            playername = PlayerName;

            audioManager = Audiomanager;
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
                    sceneManager.AddScene(new CreateLobbyScene(contentManager, sceneManager, switchPage));
                }
                else if (joinButton.CheckLeftClick(position))
                {
                    sceneManager.RemoveScene();
                    sceneManager.AddScene(new GameScene(contentManager, sceneManager, audioManager, playername));
                }else if (refreshButton.CheckLeftClick(position))
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
            spriteBatch.Draw(backGround, new Rectangle(0, 0, 1280, 720), Color.White);
            switchPage.Draw(spriteBatch);
            createButton.Draw(spriteBatch);
            joinButton.Draw(spriteBatch);
            refreshButton.Draw(spriteBatch);  

        }

    }
}

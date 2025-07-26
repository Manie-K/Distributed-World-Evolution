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
        private Texture2D BackGround;
        private SceneManager sceneManager;
        private SwitchPage SwitchPage;
        private Button CreateButton;
        private Button JoinButton;
        private Button RefreshButton;
        private MouseState previousMouseState;
        private AudioManager audioManager;
        private Player player;

        public LobbyScene(ContentManager contentManager, SceneManager sceneManager, AudioManager audiomanager, string PlayerName)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.BackGround = contentManager.Load<Texture2D>("UI/Scenes/Lobby_BG");
            this.SwitchPage = new SwitchPage(contentManager.Load<Texture2D>("UI/White Left"), contentManager.Load<Texture2D>("UI/White Right"),
                                             contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(542, 628), contentManager);
            this.JoinButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/JoinButton"), null, null, new Vector2(953, 240), 180, 70, new Color(255, 255, 128));
            this.RefreshButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/RefreshButton"), null, null, new Vector2(953, 330), 180, 70, new Color(255, 255, 128));
            this.CreateButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/CreateButton"), null, null, new Vector2(953, 420), 180, 70, new Color(255, 255, 128));
            this.player = new Player(contentManager.Load<Texture2D>("Warrior_Sheet-Effect"), new Vector2(500, 300), Color.White, 
                                     new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),PlayerName,true,new Vector2(500, 300 - 110),70,40));
            this.audioManager = audiomanager;
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
                SwitchPage.CheckLeftClick(position);
                if (CreateButton.CheckLeftClick(position))
                {
                    sceneManager.AddScene(new CreateLobbyScene(contentManager, sceneManager, SwitchPage));
                }
                else if (JoinButton.CheckLeftClick(position))
                {
                    sceneManager.RemoveScene();
                    sceneManager.AddScene(new GameScene(contentManager, sceneManager, audioManager, player));
                }else if (RefreshButton.CheckLeftClick(position))
                {
                    Debug.WriteLine("Refresh");
                }
                isPressed= true;    
            }

            CreateButton.Update(position);
            JoinButton.Update(position);
            RefreshButton.Update(position);
            SwitchPage.UpdateRows(position, isPressed);
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            SwitchPage.Draw(spriteBatch);
            CreateButton.Draw(spriteBatch);
            JoinButton.Draw(spriteBatch);
            RefreshButton.Draw(spriteBatch);  

        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class GameScene : IScene
    {
        private ContentManager contentManager;
        private Texture2D BackGround;
        private SceneManager sceneManager;
        private AudioManager audioManager;
        private MouseState previousMouseState;
        private Player player;

        public GameScene(ContentManager contentManager, SceneManager sceneManager, AudioManager audiomanager, Player player)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.BackGround = contentManager.Load<Texture2D>("UI/BG_Forest");
            this.player = player;   
            this.audioManager = audiomanager;
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {


            }

            player.Update(gameTime);
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            player.Draw(spriteBatch);
        }

    }
}

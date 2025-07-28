using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client;
using System;
using System.Diagnostics;

namespace Client
{
    public class GameScene : IScene
    {
        private ContentManager contentManager;
        private Texture2D BackGround;
        private SceneManager sceneManager;
        private AudioManager audioManager;
        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;
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
            // Możesz tu wrzucić dodatkowe dane do załadowania jeśli chcesz
        }
        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {

            }

            KeyboardState currentKeyboardState = Keyboard.GetState();


            player.Update(gameTime, currentKeyboardState, previousKeyboardState);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            player.Draw(spriteBatch);
        }
    }
}
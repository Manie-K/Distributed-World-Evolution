using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client;
using System.Diagnostics;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SceneManager sceneManager;
        private AudioManager audioManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sceneManager = new();
            audioManager = new AudioManager(Content);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            sceneManager.AddScene(new MainMenuScene(Content, sceneManager, this, audioManager));
            sceneManager.GetCurrentScene().Load();
        }

        protected override void Update(GameTime gameTime)
        {

            sceneManager.GetCurrentScene().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            sceneManager.GetCurrentScene().Draw(spriteBatch);
     

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

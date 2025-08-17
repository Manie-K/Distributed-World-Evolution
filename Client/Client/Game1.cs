using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly GameManager manager;

        public Game1()
        {
            manager = new GameManager(Content);
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = manager.UserSettings.ScreenWidth;
            graphics.PreferredBackBufferHeight = manager.UserSettings.ScreenHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use manager.ContentManager to load your game content here
            manager.SceneManager.AddScene(new MainMenuScene(manager, this));
            manager.SceneManager.GetCurrentScene().Load();
        }

        protected override void Update(GameTime gameTime)
        {
            manager.SceneManager.GetCurrentScene().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: manager.Camera.Transform, samplerState: SamplerState.PointClamp);
            manager.SceneManager.GetCurrentScene().Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

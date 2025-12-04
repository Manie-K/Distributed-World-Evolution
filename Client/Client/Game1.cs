using Client.Logic;
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

            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            manager.SetWindowManager();
            manager.ClientManager.WindowManager = manager.WindowManager;
            manager.SceneManager.AddScene(new MainMenuScene(manager, this));
            manager.SceneManager.GetCurrentScene().Load();

            AssetsManager.GetInstance().Load(manager.ContentManager);
        }

        protected override void Update(GameTime gameTime)
        {
            manager.InputManager.Update();

            if (!manager.WindowManager.Update())
            {
                manager.SceneManager.GetCurrentScene().Update(gameTime);
            }

            manager.InputManager.SetPreviousStates();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: manager.Camera.Transform, samplerState: SamplerState.PointClamp);
            manager.SceneManager.GetCurrentScene().Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            manager.SceneManager.GetCurrentScene().DrawStatic(spriteBatch);
            manager.WindowManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

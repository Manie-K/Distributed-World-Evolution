using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Rendering;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SceneManager sceneManager;
        private AudioManager audioManager;
        private ScreenSize screenSize;
        private readonly Camera2D camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sceneManager = new();
            audioManager = new AudioManager(Content);
            camera = new Camera2D(screenSize);
            screenSize = new ScreenSize(1280, 720);

            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;
            camera.CenterOn(new Vector2(screenSize.Width / 2f, screenSize.Height / 2f));
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

            spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp);
            sceneManager.GetCurrentScene().Draw(spriteBatch, camera);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

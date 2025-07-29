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

        private EnemyPlant1 enemyPlant1;
        private EnemyPlant1 enemyPlant2;
        private Pig Pig1;
        private Boar Boar1;
        private WhiteRabbit whiterabbit1;
        private BrownRabbit brownrabbit1;
        private Player player;
        private AnimationTexturesLoader animationTexturesLoader;

        public GameScene(ContentManager contentManager, SceneManager sceneManager, AudioManager audiomanager, string PlayerName)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.BackGround = contentManager.Load<Texture2D>("UI/BG_Forest");
            this.animationTexturesLoader = new AnimationTexturesLoader(contentManager);
            this.player = new Player(new Vector2(600, 200), Color.White,
                                     new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader);
            this.enemyPlant1 = new EnemyPlant1(new Vector2(200, 200), Color.White, ref this.animationTexturesLoader);
            this.enemyPlant2 = new EnemyPlant1(new Vector2(300, 200), Color.White, ref this.animationTexturesLoader);
            this.Pig1 = new Pig(new Vector2(400, 200), Color.White, ref this.animationTexturesLoader);
            this.Boar1 = new Boar(new Vector2(500, 200), Color.White, ref this.animationTexturesLoader);
            this.whiterabbit1 = new WhiteRabbit(new Vector2(700, 200), Color.White, ref this.animationTexturesLoader);
            this.brownrabbit1 = new BrownRabbit(new Vector2(800, 200), Color.White, ref this.animationTexturesLoader);
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

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                sceneManager.AddScene(new SettingsScene(contentManager, sceneManager, audioManager));
                audioManager.MuteAll();
            }


            player.Update(gameTime, currentKeyboardState, previousKeyboardState);
            enemyPlant1.Update(gameTime, currentKeyboardState, previousKeyboardState);
            enemyPlant2.Update(gameTime, currentKeyboardState, previousKeyboardState);
            Pig1.Update(gameTime, currentKeyboardState, previousKeyboardState);
            Boar1.Update(gameTime, currentKeyboardState, previousKeyboardState);
            whiterabbit1.Update(gameTime, currentKeyboardState, previousKeyboardState);
            brownrabbit1.Update(gameTime, currentKeyboardState, previousKeyboardState);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            player.Draw(spriteBatch);
            enemyPlant1.Draw(spriteBatch);
            enemyPlant2.Draw(spriteBatch);
            Pig1.Draw(spriteBatch);
            Boar1.Draw(spriteBatch);
            whiterabbit1.Draw(spriteBatch);
            brownrabbit1.Draw(spriteBatch);
        }
    }
}
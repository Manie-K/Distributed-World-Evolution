using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client;
using System;
using System.Diagnostics;
using System.Collections.Generic;

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

        private List<Character> Characters;

        private AnimationTexturesLoader animationTexturesLoader;

        public GameScene(ContentManager contentManager, SceneManager sceneManager, AudioManager audiomanager, string PlayerName)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.BackGround = contentManager.Load<Texture2D>("UI/BG_Forest");
            this.animationTexturesLoader = new AnimationTexturesLoader(contentManager);
            this.player = new Player(new Vector2(600, 200), Color.White,
                                     new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader);
            this.Characters = new List<Character>();
            this.audioManager = audiomanager;

            LoadCharacters();
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
            foreach(Character character in this.Characters)
            {
                character.Update(gameTime, currentKeyboardState, previousKeyboardState);
            }

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            player.Draw(spriteBatch);
            foreach (Character character in this.Characters)
            {
                character.Draw(spriteBatch);
            }
        }

        private void LoadCharacters()
        {
            Characters.Add(new EnemyPlant1(new Vector2(200, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new EnemyPlant2(new Vector2(300, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Pig(new Vector2(400, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Boar(new Vector2(500, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new WhiteRabbit(new Vector2(700, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new BrownRabbit(new Vector2(800, 200), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new EnemyPlant3(new Vector2(200, 350), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Slime1(new Vector2(300, 350), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Slime2(new Vector2(400, 350), Color.White, ref this.animationTexturesLoader));

            Characters.Add(new Slime3(new Vector2(500, 350), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Orc1(new Vector2(600, 350), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Orc2(new Vector2(700, 350), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Orc3(new Vector2(200, 500), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Vampire1(new Vector2(300, 500), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Vampire2(new Vector2(400, 500), Color.White, ref this.animationTexturesLoader));
            Characters.Add(new Vampire3(new Vector2(500, 500), Color.White, ref this.animationTexturesLoader));
        }
    }
}
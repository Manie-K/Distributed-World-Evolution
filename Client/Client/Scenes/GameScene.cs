using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Client
{
    public class GameScene : IScene
    {
        private GameManager manager;

        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;
        private Player player;
        private List<Character> characters;
        private AnimationTexturesLoader animationTexturesLoader;
        private Tilemap map;
        private Vector2 cameraOffset;

        public GameScene(GameManager manager)
        {
            this.manager = manager;

            animationTexturesLoader = new AnimationTexturesLoader(manager.ContentManager);
            player = new Player(new Vector2(600, 200), Color.White,
                                     new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader);
            characters = new List<Character>();

            cameraOffset = new Vector2(50, 100);
            map = new Tilemap();
            map.LoadMap("Content/Maps/map1.json", manager.ContentManager);
            manager.Camera.MapSize = new System.Drawing.Size(map.Width * map.TileSize, map.Height * map.TileSize);
            manager.IsInGame = true;
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
                manager.Camera.ResetPosition();
                manager.SceneManager.AddScene(new SettingsScene(manager));
            }


            player.Update(gameTime, currentKeyboardState, previousKeyboardState);
            foreach(Character character in characters)
            {
                character.Update(gameTime, currentKeyboardState, previousKeyboardState);
            }

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            manager.Camera.CenterOn(player.Position + cameraOffset);
            map.Draw(spriteBatch, manager.Camera);
            player.Draw(spriteBatch);
            foreach (Character character in characters)
            {
                character.Draw(spriteBatch);
            }
        }

        private void LoadCharacters()
        {
            characters.Add(new EnemyPlant1(new Vector2(200, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new EnemyPlant2(new Vector2(300, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Pig(new Vector2(400, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Boar(new Vector2(500, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new WhiteRabbit(new Vector2(700, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new BrownRabbit(new Vector2(800, 200), Color.White, ref this.animationTexturesLoader));
            characters.Add(new EnemyPlant3(new Vector2(200, 350), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Slime1(new Vector2(300, 350), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Slime2(new Vector2(400, 350), Color.White, ref this.animationTexturesLoader));

            characters.Add(new Slime3(new Vector2(500, 350), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Orc1(new Vector2(600, 350), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Orc2(new Vector2(700, 350), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Orc3(new Vector2(200, 500), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Vampire1(new Vector2(300, 500), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Vampire2(new Vector2(400, 500), Color.White, ref this.animationTexturesLoader));
            characters.Add(new Vampire3(new Vector2(500, 500), Color.White, ref this.animationTexturesLoader));
        }
    }
}
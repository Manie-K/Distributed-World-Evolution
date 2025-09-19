using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary;
using System;
using System.Collections.Generic;

namespace Client
{
    public class GameScene : IScene
    {
        private GameManager manager;

        private PanelsController panelsController;
        private Player player;
        private List<Character> characters;
        private AnimationTexturesLoader animationTexturesLoader;
        private WorldMap map;
        private Vector2 cameraOffset;
        private double clientUpdateTimer;
        private double timeBetweenUpdates;

        public GameScene(GameManager manager)
        {
            this.manager = manager;

            animationTexturesLoader = new AnimationTexturesLoader(manager.ContentManager);
            player = new Player(new Vector2(600, 200), Color.White,
                                     new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader);
            characters = new List<Character>();

            panelsController = new PanelsController(manager);
            cameraOffset = new Vector2(50, 100);
            map = new WorldMap();
            map.InitMap("Content/Maps/Grassland.json", manager.ContentManager);
            manager.Camera.MapSize = new System.Drawing.Size(map.MapWidth * map.TileSize, map.MapHeight * map.TileSize);
            manager.IsInGame = true;
            clientUpdateTimer = 0;
            timeBetweenUpdates = 1.0 / ClientManager.CLIENT_UPDATES_PER_SECOND;
           // LoadCharacters();
        }

        public void Load()
        {
            // Możesz tu wrzucić dodatkowe dane do załadowania jeśli chcesz
        }

        public void Update(GameTime gameTime)
        {
            panelsController.Update();

            player.Update(gameTime, manager.InputManager);

            IReadOnlyDictionary<Guid, WorldEntityDTO> entities = manager.ClientManager.Entities;
            foreach (Character character in characters)
            {
                //character.Update(gameTime, manager.InputManager);
                if (entities.TryGetValue(character.Id, out WorldEntityDTO entity))
                { 
                    character.Position = entity.State.Position;
                }
            }

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            clientUpdateTimer += delta;

            if (clientUpdateTimer >= timeBetweenUpdates)
            {
                //EntityStateMessage message = new EntityStateMessage(new WorldEntityDTO(System.Guid.NewGuid(), new EntityStateDTO(System.Numerics.Vector2.Zero)));
                //MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                clientUpdateTimer = 0;
            }
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

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            panelsController.Draw(spriteBatch);
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
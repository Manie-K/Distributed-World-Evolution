using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;

namespace Client
{
    public class GameScene : IScene
    {
        private GameManager manager;

        private PanelsController panelsController;
        private Player player;
        private Dictionary<Guid, Character> characters;
        private AnimationTexturesLoader animationTexturesLoader;
        private WorldMap map;
        private Vector2 cameraOffset;
        private double clientUpdateTimer;
        private double timeBetweenUpdates;
        private EntityStateDTO playerStateDTO;
        private WorldEntityDTO playerDTO;

        public GameScene(GameManager manager, int mapID)
        {
            this.manager = manager;

            animationTexturesLoader = new AnimationTexturesLoader(manager.ContentManager);
            player = new Player(new Vector2(600, 200), Color.White,
                                     new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader);
            characters = [];

            panelsController = new PanelsController(manager);
            cameraOffset = new Vector2(50, 100);
            map = new WorldMap();
            if (!map.InitMap($"Content/Maps/{GetMapFileName(mapID)}", manager.ContentManager))
            {
                throw new Exception("Could not load the map " + GetMapFileName(mapID));
            }
            manager.Camera.MapSize = new System.Drawing.Size(map.MapWidth * map.TileSize, map.MapHeight * map.TileSize);
            manager.IsInGame = true;
            clientUpdateTimer = 0;
            timeBetweenUpdates = 1.0 / ClientManager.CLIENT_UPDATES_PER_SECOND;

            playerStateDTO = new EntityStateDTO(new SharedLibrary.Helpers.Position2D(15, 15), 100, 100, 0);
            playerDTO = new WorldEntityDTO(manager.UserSettings.PlayerName, System.Guid.NewGuid(), playerStateDTO, 0);
            // LoadCharacters();
        }

        public void Load()
        {
            // Możesz tu wrzucić dodatkowe dane do załadowania jeśli chcesz
        }

        public void Update(GameTime gameTime)
        {
            panelsController.Update();

            IReadOnlyDictionary<Guid, WorldEntityDTO> entities = manager.ClientManager.Entities;
            Dictionary<Guid, Character> newCharacterList = [];

            foreach (Character character in characters.Values)
            {
                if (entities.ContainsKey(character.Id))
                {
                    newCharacterList.Add(character.Id, character);
                }
            }

            characters = newCharacterList;

            foreach (WorldEntityDTO entity in entities.Values)
            {
                if (characters.TryGetValue(entity.Id, out Character character))
                {
                    //character.Update(gameTime, manager.InputManager);
                    character.Position.X = entity.State.Position.X;
                    character.Position.Y = entity.State.Position.Y;
                }
                else
                {
                    LoadCharacter(entity);
                }
            }

            player.Update(gameTime, manager.InputManager);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            clientUpdateTimer += delta;

            if (clientUpdateTimer >= timeBetweenUpdates)
            {
                UserInteractionMessage message = new UserInteractionMessage(playerDTO, null);
                _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                clientUpdateTimer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            manager.Camera.CenterOn(player.Position + cameraOffset);
            map.Draw(spriteBatch, manager.Camera);
            player.Draw(spriteBatch);
            foreach (Character character in characters.Values)
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
            /*
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
            */
        }

        // TODO: switch based on graphicID; add all cases
        private void LoadCharacter(WorldEntityDTO entity)
        {
            Vector2 position = new Vector2(entity.State.Position.X, entity.State.Position.Y);

            switch (entity.ModuleID)
            {
                case 0:
                    characters.Add(entity.Id, new EnemyPlant1(position, Color.White, ref this.animationTexturesLoader));
                    break;
                case 1:
                    characters.Add(entity.Id, new EnemyPlant2(position, Color.White, ref this.animationTexturesLoader));
                    break;

                default:
                    characters.Add(entity.Id, new EnemyPlant1(position, Color.White, ref this.animationTexturesLoader));
                    break;
            }
        }

        private string GetMapFileName(int mapID)
        {
            string mapName = mapID switch
            {
                0 => "Grassland.json",
                _ => "null.json",
            };

            return mapName;
        }
    }
}
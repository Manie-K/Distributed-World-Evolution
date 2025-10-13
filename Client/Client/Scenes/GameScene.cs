using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private WorldEntityDTO playerDTO;

        public GameScene(GameManager manager, int mapID)
        {
            this.manager = manager;

            animationTexturesLoader = new AnimationTexturesLoader(manager.ContentManager);
            characters = [];

            panelsController = new PanelsController(manager);
            cameraOffset = new Vector2(0, 70);
            map = new WorldMap();
            if (!map.InitMap($"Content/Maps/{GetMapFileName(mapID)}", manager.ContentManager))
            {
                throw new Exception("Could not load the map " + GetMapFileName(mapID));
            }
            player = new Player(new Vector2(600, 200), Color.White, new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), 
                manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), ref this.animationTexturesLoader, map, manager.ClientManager);

            manager.Camera.MapSize = new System.Drawing.Size(map.MapWidth * map.TileSize, map.MapHeight * map.TileSize);
            manager.IsInGame = true;
            clientUpdateTimer = 0;
            timeBetweenUpdates = 1.0 / ClientManager.CLIENT_UPDATES_PER_SECOND;

            EntityStateDTO playerStateDTO = new EntityStateDTO(map.GetTilePosition2D(player.Position.X, player.Position.Y), 100, 100, 0);
            playerDTO = new WorldEntityDTO(manager.UserSettings.PlayerName, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), playerStateDTO, 0);
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
                    character.Position = GetWorldPosition(entity);
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
                playerDTO.State.Position = map.GetTilePosition2D(player.Position.X, player.Position.Y);
                UserInteractionMessage message = new UserInteractionMessage(playerDTO, player.TargetEntity);
                _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                clientUpdateTimer = 0;
                player.TargetEntity = null;
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

        private void LoadCharacter(WorldEntityDTO entity)
        {
            int graphicID = manager.ClientManager.LobbyData.Modules.FirstOrDefault(m => m.DatabaseID == entity.ModuleID)?.GraphicalRepresentationID ?? -1;
            Vector2 position = GetWorldPosition(entity);

            characters.Add(entity.Id, graphicID switch
            { 
                0 => new EnemyPlant1(position, Color.White, ref this.animationTexturesLoader),
                1 => new EnemyPlant2(position, Color.White, ref this.animationTexturesLoader),
                2 => new Pig(position, Color.White, ref this.animationTexturesLoader),
                3 => new Boar(position, Color.White, ref this.animationTexturesLoader),
                4 => new WhiteRabbit(position, Color.White, ref this.animationTexturesLoader),
                5 => new BrownRabbit(position, Color.White, ref this.animationTexturesLoader),
                6 => new EnemyPlant3(position, Color.White, ref this.animationTexturesLoader),
                7 => new Slime1(position, Color.White, ref this.animationTexturesLoader),
                8 => new Slime2(position, Color.White, ref this.animationTexturesLoader),
                9 => new Slime3(position, Color.White, ref this.animationTexturesLoader),
                10 => new Orc1(position, Color.White, ref this.animationTexturesLoader),
                11 => new Orc2(position, Color.White, ref this.animationTexturesLoader),
                12 => new Orc3(position, Color.White, ref this.animationTexturesLoader),
                13 => new Vampire1(position, Color.White, ref this.animationTexturesLoader),
                14 => new Vampire2(position, Color.White, ref this.animationTexturesLoader),
                15 => new Vampire3(position, Color.White, ref this.animationTexturesLoader),
                _ => new EnemyPlant1(position, Color.White, ref this.animationTexturesLoader)
            });
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

        private Vector2 GetWorldPosition(WorldEntityDTO entity)
        {
            return new Vector2(entity.State.Position.X * map.TileSize, entity.State.Position.Y * map.TileSize);
        }
    }
}
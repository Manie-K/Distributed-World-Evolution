using Client.Logic.Plants;
using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary;
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
        private Dictionary<Guid, Plant> plants;
        private WorldMap map;
        private Vector2 cameraOffset;
        private double clientUpdateTimer;
        private double timeBetweenUpdates;

        public GameScene(GameManager manager, int mapID)
        {
            this.manager = manager;

            characters = [];
            plants = [];

            panelsController = new PanelsController(manager);
            cameraOffset = new Vector2(0, 70);
            map = new WorldMap();
            if (!map.InitMap($"Content/Maps/", mapID, manager.ContentManager))
            {
                throw new Exception("Could not load the map " + Tilemap.GetMapFileName(mapID));
            }
            player = new Player(new Vector2(288, 32), Color.White, new Text(manager.ContentManager.Load<SpriteFont>("Fonts/PlayerName"), 
                manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), new Vector2(-68, -77),
                ref panelsController.BestiaryPanel, ref panelsController.Inventory, map, manager.ClientManager);

            manager.Camera.MapSize = new System.Drawing.Size(map.MapWidth * map.TileSize, map.MapHeight * map.TileSize);
            manager.IsInGame = true;
            clientUpdateTimer = 0;
            timeBetweenUpdates = 1.0 / ClientManager.CLIENT_UPDATES_PER_SECOND;
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            panelsController.Update();
            
            IReadOnlyDictionary<Guid, WorldEntityDTO> entities = manager.ClientManager.Entities;
            Dictionary<Guid, Character> newCharacterList = [];
            Dictionary<Guid, Plant> newPlantList = [];

            // Removing dead creatures
            foreach (Guid guid in characters.Keys)
            {
                if (entities.ContainsKey(guid))
                {
                    newCharacterList.Add(guid, characters[guid]);
                }
            }
            characters = newCharacterList;

            // Removing dead plants
            foreach (Guid guid in plants.Keys)
            {
                if (entities.ContainsKey(guid))
                {
                    newPlantList.Add(guid, plants[guid]);
                }
            }
            plants = newPlantList;

            foreach (WorldEntityDTO entity in entities.Values)
            {
                if (entity.Id.Equals(manager.ClientManager.PlayerGuid))
                {
                    player.PlayerDTO ??= entity;
                    player.PlayerDTO.State.Health = entity.State.Health;
                    panelsController.SetHealthBarValue(entity.State.Health / player.GetPlayerMaxHealth());
                    continue;
                }

                if (characters.TryGetValue(entity.Id, out Character character))
                {
                    character.Update(gameTime, manager.InputManager, entity.State);
                    character.SetCurrentDirection(map.GetTilePosition2D(character.Position.X, character.Position.Y), entity.State.Position);
                    character.Position = GetWorldPosition(entity);
                }
                else if (plants.TryGetValue(entity.Id, out Plant plant))
                {
                    plant.Position = GetWorldPosition(entity);
                }
                else
                {
                    LoadEntity(entity);
                }
            }

            player.Update(gameTime, manager.InputManager);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            clientUpdateTimer += delta;

            if (clientUpdateTimer >= timeBetweenUpdates && player.PlayerDTO != null)
            {
                player.PlayerDTO.State.Position = map.GetTilePosition2D(player.Position.X, player.Position.Y);
                UserInteractionMessage message = new UserInteractionMessage(player.PlayerDTO, player.TargetEntity);
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
            
            foreach(Plant plant in plants.Values)
            {
                plant.Draw(spriteBatch);
            }
        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            panelsController.Draw(spriteBatch);
        }

        private void LoadEntity(WorldEntityDTO entity)
        {
            int graphicID = manager.ClientManager.Modules.FirstOrDefault(m => m.DatabaseID == entity.ModuleID)?.GraphicalRepresentationID ?? -1;
            Vector2 position = GetWorldPosition(entity);

            if (graphicID <= 16)
            {
                characters.Add(entity.Id, graphicID switch
                {
                    0 => new EnemyPlant1(position, Color.White),
                    1 => new EnemyPlant2(position, Color.White),
                    2 => new Pig(position, Color.White),
                    3 => new Boar(position, Color.White),
                    4 => new WhiteRabbit(position, Color.White),
                    5 => new BrownRabbit(position, Color.White),
                    6 => new EnemyPlant3(position, Color.White),
                    7 => new Slime1(position, Color.White),
                    8 => new Slime2(position, Color.White),
                    9 => new Slime3(position, Color.White),
                    10 => new Orc1(position, Color.White),
                    11 => new Orc2(position, Color.White),
                    12 => new Orc3(position, Color.White),
                    13 => new Vampire1(position, Color.White),
                    14 => new Vampire2(position, Color.White),
                    15 => new Vampire3(position, Color.White),
                    16 => new Player(position, Color.White, new Text(manager.ContentManager.Load<SpriteFont>("Fonts/PlayerName"), entity.Name,
                    true, new Vector2(500, 300 - 110), 70, 40), new Vector2(-53, -50), ref panelsController.BestiaryPanel, ref panelsController.Inventory),
                    _ => new EnemyPlant1(position, Color.White)
                });
            }
            else
            {
                plants.Add(entity.Id, graphicID switch
                {
                    17 => new Cosmo(position),
                    18 => new Daffodil(position),
                    19 => new Daisy(position),
                    20 => new Lavender(position),
                    21 => new Lily(position),
                    22 => new LilyOfTheValley(position),
                    23 => new Orchid(position),
                    24 => new Pansy(position),
                    25 => new Poppy(position),
                    26 => new Rose(position),
                    27 => new Sunflower(position),
                    28 => new Tulip(position),
                    _ => new Cosmo(position)
                });
            }
        }

        private Vector2 GetWorldPosition(WorldEntityDTO entity)
        {
            return new Vector2(entity.State.Position.X * map.TileSize, entity.State.Position.Y * map.TileSize);
        }
    }
}
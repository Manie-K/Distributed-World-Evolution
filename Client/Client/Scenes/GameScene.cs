using Client.Logic.Plants;
using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    public class GameScene : IScene
    {
        private GameManager manager;
        private Text performanceText;
        private PanelsController panelsController;
        private Player player;
        private Dictionary<Guid, Character> characters;
        private Dictionary<Guid, Plant> plants;
        private List<WorldEntityDTO> inGameEntities;
        private WorldMap map;
        private Vector2 cameraOffset;

        private bool showPerformance;
        private double clientUpdateTimer;
        private double timeBetweenUpdates;

        public GameScene(GameManager manager, int mapID)
        {
            this.manager = manager;
            characters = [];
            plants = [];

            performanceText = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "sec", false, new Vector2(900, 10), 106, 40);
            panelsController = new PanelsController(manager);
            cameraOffset = new Vector2(0, 70);
            map = new WorldMap();
            if (!map.InitMap($"Content/Maps/", mapID, manager.ContentManager))
            {
                throw new Exception("Could not load the map " + Tilemap.GetMapFileName(mapID));
            }
            player = new Player(new Vector2(40, 40), Color.White, new Text(manager.ContentManager.Load<SpriteFont>("Fonts/PlayerName"), 
                manager.UserSettings.PlayerName, true, new Vector2(500, 300 - 110), 70, 40), new Vector2(-68, -77), -1,
                ref panelsController.BestiaryPanel, ref panelsController.Inventory, map, manager.ClientManager);

            manager.Camera.MapSize = new System.Drawing.Size(map.MapWidth * map.TileSize, map.MapHeight * map.TileSize);
            manager.IsInGame = true;
            clientUpdateTimer = 0;
            timeBetweenUpdates = 1.0 / ClientManager.CLIENT_UPDATES_PER_SECOND;
            showPerformance = bool.Parse(manager.AppConfig["TcpSettings:ShowPerformance"]);

            inGameEntities = new List<WorldEntityDTO>();
            List<WorldEntityDTO> entitiesToLoad = manager.ClientManager.LobbyData.WorldEntities.ToList();
            foreach (WorldEntityDTO entity in entitiesToLoad)
            {
                if (entity.Id.Equals(manager.ClientManager.PlayerGuid))
                {
                    SetPlayerStats(entity);
                    continue;
                }
                LoadEntity(entity);
            }
        }

        public void Load() {}

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (showPerformance) performanceText.SetText("sec: " + delta + " ; creatures: " + inGameEntities.Count);

            panelsController.Update();
            
            IReadOnlyDictionary<Guid, WorldEntityDTO> entities = manager.ClientManager.FlushEntities();
            Dictionary<Guid, Character> newCharacterList = [];
            Dictionary<Guid, Plant> newPlantList = [];

            // Removing dead creatures
            foreach (Guid guid in characters.Keys)
            {
                if (!characters[guid].IsDead)
                {
                    newCharacterList.Add(guid, characters[guid]);
                }
                else
                {
                    inGameEntities.RemoveAll(e => e.Id == guid);
                }
            }
            characters = newCharacterList;

            // Removing dead plants
            foreach (Guid guid in plants.Keys)
            {
                if (!plants[guid].IsDead)
                {
                    newPlantList.Add(guid, plants[guid]);
                }
                else
                {
                    inGameEntities.RemoveAll(e => e.Id == guid);
                }
            }
            plants = newPlantList;

            // Updating new state
            foreach (WorldEntityDTO entity in entities.Values)
            {
                if (entity.Id.Equals(manager.ClientManager.PlayerGuid))
                {
                    SetPlayerStats(entity);
                    continue;
                }

                if (characters.TryGetValue(entity.Id, out Character character))
                {
                    character.Update(gameTime, entity.State);
                    character.SetCurrentDirection(map.GetTilePosition2D(character.Position.X, character.Position.Y), entity.State.Position);
                    character.Position = GetWorldPosition(entity);
                    character.HealthBar.SetRangeBar((float) entity.State.Health / character.MaxHealth);
                    int index = inGameEntities.FindIndex(e => e.Id == entity.Id);
                    if (index != -1)
                    {
                        inGameEntities[index] = entity;
                    }
                }
                else if (plants.TryGetValue(entity.Id, out Plant plant))
                {
                    plant.Position = GetWorldPosition(entity);
                    if (entity.State.Health <= 0)
                    { 
                        plant.IsDead = true;
                    }
                    int index = inGameEntities.FindIndex(e => e.Id == entity.Id);
                    if (index != -1)
                    {
                        inGameEntities[index] = entity;
                    }
                }
                else if (entity.State.Health > 0)
                {
                    LoadEntity(entity);
                }
            }

            // Updating creatures without new state
            foreach (Guid guid in characters.Keys)
            {
                if (!entities.ContainsKey(guid))
                {
                    characters[guid].Update(gameTime);
                }
            }

            // Updating player
            player.Update(gameTime, manager.InputManager, inGameEntities);
            clientUpdateTimer += delta;
            SendPlayerStatus();
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
            if (showPerformance) performanceText.Draw(spriteBatch);
            panelsController.Draw(spriteBatch);
        }

        private void LoadEntity(WorldEntityDTO entity)
        {
            inGameEntities.Add(entity);
            int graphicID = manager.ClientManager.Modules.FirstOrDefault(m => m.DatabaseID == entity.ModuleID)?.GraphicalRepresentationID ?? -1;
            int maxHealth = manager.ClientManager.Modules.FirstOrDefault(m => m.DatabaseID == entity.ModuleID)?.MaxHealth ?? -1;
            Vector2 position = GetWorldPosition(entity);

            if (graphicID <= 16)
            {
                characters.Add(entity.Id, graphicID switch
                {
                    0 => new EnemyPlant1(position, maxHealth, Color.White),
                    1 => new EnemyPlant2(position, maxHealth, Color.White),
                    2 => new Pig(position, maxHealth, Color.White),
                    3 => new Boar(position, maxHealth, Color.White),
                    4 => new WhiteRabbit(position, maxHealth, Color.White),
                    5 => new BrownRabbit(position, maxHealth, Color.White),
                    6 => new EnemyPlant3(position, maxHealth, Color.White),
                    7 => new Slime1(position, maxHealth, Color.White),
                    8 => new Slime2(position, maxHealth, Color.White),
                    9 => new Slime3(position, maxHealth, Color.White),
                    10 => new Orc1(position, maxHealth, Color.White),
                    11 => new Orc2(position, maxHealth, Color.White),
                    12 => new Orc3(position, maxHealth, Color.White),
                    13 => new Vampire1(position, maxHealth, Color.White),
                    14 => new Vampire2(position, maxHealth, Color.White),
                    15 => new Vampire3(position, maxHealth, Color.White),
                    16 => new Player(position, Color.White, new Text(manager.ContentManager.Load<SpriteFont>("Fonts/PlayerName"), entity.Name,
                    true, new Vector2(500, 300 - 110), 70, 40), new Vector2(-53, -50), maxHealth, ref panelsController.BestiaryPanel, ref panelsController.Inventory),
                    _ => new EnemyPlant1(position, maxHealth, Color.White)
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

        private void SendPlayerStatus()
        {
            if (clientUpdateTimer < timeBetweenUpdates || player.PlayerDTO == null) return;

            int playerHealth = player.PlayerDTO.State.Health;
            Position2D targetEntityPosition = null;
            player.PlayerDTO.State.Position = map.GetTilePosition2D(player.Position.X, player.Position.Y) - player.LastPosition;
            player.LastPosition = map.GetTilePosition2D(player.Position.X, player.Position.Y);
            if (playerHealth <= 0)
            {
                player.PlayerDTO.State.Health = (int)player.GetPlayerMaxHealth();
                player.PlayerDTO.State.Hunger = (int)player.GetPlayerMaxHunger();
            }
            else
            {
                player.PlayerDTO.State.Hunger = player.HungerToConsume;
                player.PlayerDTO.State.Health = 0;
            }
            if (player.TargetEntity != null)
            {
                targetEntityPosition = player.TargetEntity.State.Position;
                player.TargetEntity.State.Position = new Position2D(0, 0);
            }

            UserInteractionMessage message = new UserInteractionMessage(player.PlayerDTO, player.TargetEntity);
            _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, message);

            if (player.TargetEntity != null)
            {
                player.TargetEntity.State.Position = targetEntityPosition;
                player.TargetEntity.State.Health = 1;
                player.TargetEntity = null;
            }
            clientUpdateTimer = 0;
            player.HungerToConsume = 0;
            player.PlayerDTO.State.Health = playerHealth;
        }

        private void SetPlayerStats(WorldEntityDTO entity)
        {
            player.PlayerDTO ??= entity;
            player.PlayerDTO.State.Health = entity.State.Health;
            player.PlayerDTO.State.Hunger = entity.State.Hunger;
            panelsController.StatsPanel.SetHealthBar(entity.State.Health / player.GetPlayerMaxHealth());
            panelsController.StatsPanel.SetHungerBar(entity.State.Hunger / player.GetPlayerMaxHunger());
        }

        private Vector2 GetWorldPosition(WorldEntityDTO entity)
        {
            return new Vector2(entity.State.Position.X * map.TileSize, entity.State.Position.Y * map.TileSize);
        }
    }
}
using Client.Common;
using Client.Logic;
using Client.Panels;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    public class Player : Character
    {
        private const float PLAYER_ACTION_COOLDOWN = 1.0f;

        public WorldEntityDTO TargetEntity;
        public WorldEntityDTO PlayerDTO;
        public Position2D LastPosition;
        public int HungerToConsume;

        private BestiaryPanel bestiaryPanel;
        private Inventory inventory;
        private Text playerName;
        private Vector2 playerNameOffset;
        private ModuleDTO playerModule;
        private readonly WorldMap map;
        private readonly ClientManager clientManager;
        private float actionCooldown;

        private enum InteractionType
        {
            Attack,
            Gather,
            Tame,
            Eat
        }

        public Player(Vector2 position, Color color, Text playerName, Vector2 spriteDrawingOffset, int maxHealth, ref BestiaryPanel bestiaryPanel,
            ref Inventory inventory, WorldMap map = null, ClientManager clientManager = null)
            : base(position, color, 130, 108, 150f, maxHealth)
        {
            this.playerName = playerName;
            SpriteDrawingOffset = spriteDrawingOffset;

            if(maxHealth == -1) playerNameOffset = new Vector2(31 + SpriteDrawingOffset.X, -3 + SpriteDrawingOffset.Y);
            else playerNameOffset = new Vector2(31 + SpriteDrawingOffset.X, -11 + SpriteDrawingOffset.Y);

            this.map = map;
            this.clientManager = clientManager;
            PlayerDTO = null;
            TargetEntity = null;
            LastPosition = new Position2D(1, 1);
            am = new AnimationManager(13);
            playerModule = null;
            actionCooldown = 0;
            this.bestiaryPanel = bestiaryPanel;
            this.inventory = inventory;
            HungerToConsume = 0;
        }

        public void Update(GameTime gameTime, InputManager inputManager = null, Dictionary<Guid, WorldEntityDTO> inGameEntities = null, EntityStateDTO state = null)
        {
            if (map == null && state == null)
            {
                base.Update(gameTime);
            }
            else if (map == null)
            {
                base.Update(gameTime, state);
            }
            else
            {
                UpdatePlayer(gameTime, inputManager, inGameEntities);
            }
        }

        private void UpdatePlayer(GameTime gameTime, InputManager inputManager, Dictionary<Guid, WorldEntityDTO> inGameEntities)
        {
            if (playerModule == null && PlayerDTO != null)
            {
                playerModule = clientManager.Modules.Where(m => m.DatabaseID == PlayerDTO.ModuleID).FirstOrDefault();
            }

            if (PlayerDTO != null && PlayerDTO.State.Health <= 0)
            {
                Position = new Vector2(40, 40);
                am.Update(gameTime);
                return;
            }

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (actionCooldown > 0)
            {
                actionCooldown -= delta;
            }

            Vector2 movement = Vector2.Zero;
            if (inputManager.CheckIfPressingKey(Keys.W))
            {
                currentDirection = Direction.Up;
                movement.Y -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.S))
            {
                currentDirection = Direction.Down;
                movement.Y += 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.A))
            {
                currentDirection = Direction.Left;
                movement.X -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.D))
            {
                currentDirection = Direction.Right;
                movement.X += 1;
            }

            SetAnimation(0);

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Vector2 newPosition = Position + (movement * speed * delta);
                SetAnimation(0);

                if (newPosition.X < 0) newPosition.X = 0;
                else if (newPosition.X >= map.MapWidth * map.TileSize) newPosition.X = map.MapWidth * map.TileSize - 1;

                if (newPosition.Y < 0) newPosition.Y = 0;
                else if (newPosition.Y >= map.MapHeight * map.TileSize) newPosition.Y = map.MapHeight * map.TileSize - 1;

                int tileID = map.GetTileIdAtPosition(newPosition.X, newPosition.Y);
                if (map.TilesetData.First(m => m.Id == tileID).Walkable)
                {
                    Position = newPosition;
                }
            }

            if (inputManager.CheckIfPressingKey(Keys.Space))
            {
                HandleInteraction(InteractionType.Attack, inGameEntities);
                SetAnimation(AnimationType.Attacking);
            }
            if (inputManager.CheckIfPressingKey(Keys.E))
            {
                HandleInteraction(InteractionType.Gather, inGameEntities);
            }
            if (inputManager.CheckIfPressingKey(Keys.F))
            {
                HandleInteraction(InteractionType.Eat, inGameEntities);
            }
            if (inputManager.CheckIfPressingKey(Keys.R))
            {
                HandleInteraction(InteractionType.Tame, inGameEntities);
            }

            am.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 13), GetPosition(), GetSourceRectangle(), Color.White);
            playerName.Draw(spriteBatch, Position + playerNameOffset);
            if(MaxHealth != -1) HealthBar.Draw(spriteBatch, Position, -7, -29);
        }

        public float GetPlayerMaxHealth()
        {
            if (playerModule != null)
            {
                return playerModule.MaxHealth;
            }
            else
            {
                return 1.0f;
            }
        }

        public float GetPlayerMaxHunger()
        {
            if (playerModule != null)
            {
                return playerModule.MaxHunger;
            }
            else
            {
                return 1.0f;
            }
        }

        private void HandleInteraction(InteractionType interactionType, Dictionary<Guid, WorldEntityDTO> inGameEntities)
        {
            if (actionCooldown > 0) return;

            TargetEntity = inGameEntities.Values.FirstOrDefault(e => e.State.Position.Equals(map.GetTilePosition2D(Position.X, Position.Y)));
            if (TargetEntity == null)
            {
                Position2D targetPosition = map.GetTilePosition2D(Position.X, Position.Y);
                switch (currentDirection)
                {
                    case Direction.Up:
                        if (targetPosition.Y > 0) targetPosition.Y -= 1;
                        break;
                    case Direction.Down:
                        if (targetPosition.Y < map.MapHeight - 1) targetPosition.Y += 1;
                        break;
                    case Direction.Left:
                        if (targetPosition.X > 0) targetPosition.X -= 1;
                        break;
                    case Direction.Right:
                        if (targetPosition.X < map.MapWidth - 1) targetPosition.X += 1;
                        break;
                    default:
                        break;
                }
                TargetEntity = inGameEntities.Values.FirstOrDefault(e => e.State.Position.Equals(targetPosition));
            }

            if (interactionType == InteractionType.Eat)
            {
                Eat();
                TargetEntity = null;
            }
            else if (TargetEntity != null && PlayerDTO != null && TargetEntity.Id != PlayerDTO.Id)
            {
                TargetEntity.State.Hunger = 0;
                switch (interactionType)
                {
                    case InteractionType.Attack:
                        AttackTarget();
                        break;
                    case InteractionType.Gather:
                        GatherTarget();
                        break;
                    case InteractionType.Tame:
                        TameTarget();
                        break;
                    default:
                        break;
                }
            }

            actionCooldown = PLAYER_ACTION_COOLDOWN;
        }

        private void Eat()
        {
            if (inventory.Eat())
            {
                HungerToConsume += 20;
            }
        }

        private void AttackTarget()
        {
            TargetEntity.State.Health = -playerModule.Damage;
            Console.WriteLine("Entity attacked for damage: " + playerModule.Damage);
        }

        private void GatherTarget()
        {
            int graphicID = clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().GraphicalRepresentationID;
            if (graphicID - 17 >= 0 && inventory.CollectItem(graphicID - 17))
            {
                TargetEntity.State.Health = -clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().MaxHealth;
                Console.WriteLine("Plant gathered");
            }
        }

        private void TameTarget()
        {
            int graphicID = clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().GraphicalRepresentationID;
            if (graphicID < 16 && inventory.RemoveOneItem() && bestiaryPanel.AddSlot(graphicID))
            {
                TargetEntity.State.Health = -clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().MaxHealth;
                Console.WriteLine("Animal tamed");
            }
        }
    }
}